from flask import Flask, render_template, request, jsonify
from pymongo import MongoClient
from datetime import datetime
import certifi
from dotenv import load_dotenv
import os

load_dotenv()

app = Flask(__name__)

# =========================
# MONGODB CONFIG
# =========================
uri = os.getenv("MONGO_URI")
client = MongoClient(uri, tls=True, tlsCAFile=certifi.where())

db = client['EduKitDB']

# Collection terpisah
pir_col = db['pir']
ultra_col = db['ultrasonic']
arduino_col = db['arduino']

# Collection history (gabungan)
history_col = db['history']


# =========================
# ROUTES PAGE
# =========================
@app.route('/')
def index():
    return render_template('index.html')

@app.route('/api/get-detail/<jenis>/<komponen>', methods=['GET'])
def get_detail(jenis, komponen):
    try:
        if jenis == "PIR":
            col = pir_col
        elif jenis == "Ultrasonic":
            col = ultra_col
        elif jenis == "Arduino":
            col = arduino_col
        else:
            return jsonify({"status": "error", "pesan": "Jenis tidak valid"})

        # 🔥 INI YANG DIUBAH
        data = col.find_one({"key": komponen}, {"_id": 0})
        
        if data:
            return jsonify(data)
        else:
            return jsonify({"status": "error", "pesan": "Data tidak ditemukan"})

    except Exception as e:
        return jsonify({"status": "error", "pesan": str(e)})
    

    
@app.route('/pir')
def pir():
    return render_template('pir.html')


@app.route('/ultrasonic')
def ultrasonic():
    return render_template('ultrasonic.html')


@app.route('/arduino')
def arduino():
    return render_template('arduino.html')


# =========================
# API SUBMIT DATA
# =========================
@app.route('/api/submit-deskripsi', methods=['POST'])
def submit_deskripsi():
    try:
        data = request.get_json()

        jenis = data.get('jenis')          # PIR / Ultrasonic / Arduino
        komponen = data.get('komponen')    # contoh: Lensa Fresnel
        deskripsi = data.get('deskripsi')

        # Validasi
        if not jenis or not komponen or not deskripsi:
            return jsonify({
                "status": "error",
                "pesan": "Data tidak lengkap!"
            })

        # =========================
        # PILIH COLLECTION
        # =========================
        if jenis == "PIR":
            target_col = pir_col
        elif jenis == "Ultrasonic":
            target_col = ultra_col
        elif jenis == "Arduino":
            target_col = arduino_col
        else:
            return jsonify({
                "status": "error",
                "pesan": "Jenis tidak dikenali!"
            })

        # =========================
        # UPDATE DATA (AUTO UPDATE)
        # =========================
        target_col.update_one(
            {"komponen": komponen},
            {"$set": {"deskripsi": deskripsi}},
            upsert=True  # kalau belum ada → insert
        )

        # =========================
        # SIMPAN KE HISTORY
        # =========================
        history_col.insert_one({
            "jenis": jenis,
            "komponen": komponen,
            "deskripsi": deskripsi,
            "waktu": datetime.now().strftime("%Y-%m-%d %H:%M:%S")
        })

        return jsonify({
            "status": "sukses",
            "pesan": "Data berhasil disimpan & history tercatat!"
        })

    except Exception as e:
        return jsonify({
            "status": "error",
            "pesan": str(e)
        })


# =========================
# API AMBIL DATA (BUAT UNITY)
# =========================
@app.route('/api/get-data/<jenis>', methods=['GET'])
def get_data(jenis):
    try:
        if jenis == "PIR":
            data = list(pir_col.find({}, {"_id": 0}))
        elif jenis == "Ultrasonic":
            data = list(ultra_col.find({}, {"_id": 0}))
        elif jenis == "Arduino":
            data = list(arduino_col.find({}, {"_id": 0}))
        else:
            return jsonify({"status": "error", "pesan": "Jenis tidak valid"})

        return jsonify(data)

    except Exception as e:
        return jsonify({"status": "error", "pesan": str(e)})


# =========================
# API HISTORY (OPTIONAL)
# =========================
@app.route('/api/history', methods=['GET'])
def get_history():
    try:
        data = list(history_col.find({}, {"_id": 0}))
        return jsonify(data)
    except Exception as e:
        return jsonify({"status": "error", "pesan": str(e)})


# =========================
# RUN APP
# =========================
if __name__ == '__main__':
     app.run(host='0.0.0.0', port=5000, debug=True)