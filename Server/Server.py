import socket
import mysql.connector
from datetime import datetime
from _thread import *
from random import randint


def establishConnections(port: int):
    sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    try:
        sock.bind(("", port))
    except socket.error as e:
        print(str(e))
        exit()
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
    myid = -1
    myteam = -1
    readyCount = 0
    isBegin = False
    print("Connected to:", addr)
    while True:
        data = conn.recv(1024).decode()
        if not data:
            break
        data = data.split(" ")

        if (data[0] == "Lobby"):
            if (isBegin):
                conn.sendall("Begin".encode())
            else:
                response = ""
                for team in teams:
                    response += str(team) + "-"
                    if (len(teams[team]) == 0):
                        response += "Empty"
                    else:
                        for player in teams[team]:
                            response += players[player]["username"] + \
                                "-" + players[player]["status"] + "-"
                    if (response[-1] == "-"):
                        response = response[:-1]
                    response += " "
                response = response[:-1]
                conn.sendall(response.encode())

        elif (data[0] == "Hello"):
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
                myid = result[0][0]
                players[myid] = {
                    "id": myid,
                    "username": result[0][1],
                    "status": "login",
                    "pos": [0, 0, 0]
                }
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
            while True:
                team = randint(1, 4)
                if (len(teams[team]) < 3):
                    teams[team].append(myid)
                    myteam = team
                    break
            players[myid]["status"] = "ready"
            response = str(result[0][0]) + " " + str(team)
            conn.sendall(response.encode())

        elif (data[0] == "Join"):
            sql.execute("SELECT id FROM games WHERE id = %s", (data[1],))
            result = sql.fetchall()
            if (len(result) == 0):
                conn.sendall("Failed".encode())
            else:
                while True:
                    team = randint(1, 4)
                    # TODO testing change to 3
                    if (len(teams[team]) < 3):
                        teams[team].append(myid)
                        myteam = team
                        break
                response = str(result[0][0]) + " " + str(team)
                players[myid]["status"] = "ready"
                conn.sendall(response.encode())

        elif (data[0] == "Status"):
            if (players[myid]["status"] == "ready"):
                players[myid]["status"] = "notready"
            else:
                players[myid]["status"] = "ready"
            conn.sendall("Done".encode())

        elif (data[0] == "isBegin"):
            for player in players.values():
                if (player["status"] == "ready"):
                    readyCount += 1
            # FIXME testing change to 12
            print(readyCount)
            if (readyCount == 4):
                print("Beginning signal sent")
                conn.sendall("Yes".encode())
            else:
                conn.sendall("No".encode())
                readyCount = 0

        elif (data[0] == "Start"):
            isBegin = True
            print("Change server begin data")
            conn.sendall("Done".encode())
    conn.close()


sock, db = establishConnections(3389)
players = {}
teams = {
    1: [],
    2: [],
    3: [],
    4: []
}

while True:
    conn, addr = sock.accept()
    start_new_thread(worker, (conn, addr, db))
