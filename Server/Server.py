import socket
import mysql.connector
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
    cursor = db.cursor()
    sock.listen()
    print("Server Started...")
    return sock, cursor


def worker(conn, addr, sql):
    global players
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
                    "INSERT INTO players (username, password, wins, losses) VALUES (%s, %s, %s, %s)", (data[1], data[2], 0, 0))
                conn.sendall("Done".encode())


    conn.close()


sock, sql = establishConnections(3389)
players = []

while True:
    conn, addr = sock.accept()
    start_new_thread(worker, (conn, addr, sql))
