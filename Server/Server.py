import socket
import mysql.connector



port = 3389
sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
# connect to database with host localhost, user root, password 123456789, database gtutag3d
db = mysql.connector.connect(
    host="localhost", user="root", passwd="12345678", database="gtutag3d")
if (db.is_connected()):
    print("Connected to database")
else:
    print("Failed to connected to database")
    exit(1)

# create cursor to execute sql commands
cursor = db.cursor()
# get latest id from table players
cursor.execute("SELECT id FROM players ORDER BY id DESC LIMIT 1")
# get the result
result = cursor.fetchone()
# print the result
print(result)
print(type(result))


sock.bind(("", port))
sock.listen()
conn, addr = sock.accept()
print("Connected to:", addr)
data = conn.recv(1024).decode()
print(data)
conn.send("Estabilished connection in server".encode())
