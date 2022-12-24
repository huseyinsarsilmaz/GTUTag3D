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
        exit(1)
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
        elif (data == "Hello"):
            conn.sendall("Hi".encode())

    conn.close()


sock, sql = establishConnections(3389)
players = []

while True:
    conn, addr = sock.accept()
    start_new_thread(worker, (conn, addr, sql))
