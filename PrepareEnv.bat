ECHO Must Run As Administrator!

netsh http add urlacl url=http://+:8080/nancy/ user=Everyone
netsh http add urlacl url=http://127.0.0.1:8080/nancy/ user=Everyone