import socket
import mysql.connector
from datetime import datetime
from _thread import *


def establishConnections(port: int):
    sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    try:
        sock.bind(("", port))
    except socket.error as e:
        print(str(e))
        exit(1)
    db = mysql.connector.connect(
        host="localhost", user="root", passwd="12345678", database="gtutag3d")
    if (db.is_connected()):
        print("Connected to database")
    else:
        print("Failed to connected to database")
        exit()
    sock.listen()
    print("Server Started...")
    return sock, db


def worker(conn, addr, db):
    global players
    sql = db.cursor()
    print("Connected to:", addr)
    while True:
        data = conn.recv(1024).decode()
        if not data:
            break
        data = data.split(" ")
        if (data[0] == "Hello"):
            conn.sendall("Hi".encode())

        elif (data[0] == "Signup"):
            sql.execute(
                "SELECT * FROM players WHERE username = %s", (data[1],))
            result = sql.fetchall()
            if (len(result) > 0):
                conn.sendall("Taken".encode())
            else:
                sql.execute(
                    "INSERT INTO players (username, password, wins, loses) VALUES (%s, %s, %s, %s)", (data[1], data[2], 0, 0))
                db.commit()
                conn.sendall("Done".encode())

        elif (data[0] == "Login"):
            sql.execute(
                "SELECT * FROM players WHERE username = %s AND password = %s", (data[1], data[2]))
            result = sql.fetchall()
            if (len(result) > 0):
                print(result)
                conn.sendall("Done".encode())
            else:
                conn.sendall("Failed".encode())
        elif (data[0] == "Create"):
            now = datetime.now()
            current_time = now.strftime("%Y-%m-%d %H:%M:%S")
            sql.execute(
                "INSERT INTO games (date, players, winners) VALUES (%s, %s, %s)", (current_time, "N N N N N N N N N N N N", "N N N"))
            result = sql.fetchall()
            db.commit()
            sql.execute("SELECT id FROM games ORDER BY id DESC LIMIT 1")
            result = sql.fetchall()
            conn.sendall(str(result[0][0]).encode())


    conn.close()


sock, db = establishConnections(3389)
players = []

while True:
    conn, addr = sock.accept()
    start_new_thread(worker, (conn, addr, db))
