import socket

ip = "34.125.80.14"
port = 3389

sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
sock.bind((ip, port))
sock.listen()
conn, addr = sock.accept()
print("Connected to:", addr)
data = conn.recv(1024).decode()
print(data)
conn.send("Estabilished connection in server".encode())
