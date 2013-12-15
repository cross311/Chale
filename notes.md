
install templates

Just runing Nancy I get an error: Nancy.Hosting.Self.AutomaticUrlReservationCreationFailureException

Must run netsh commands in As Administrator cmd.exe
```
netsh http add urlacl url=http://+:8888/nancy/ user=Everyone
netsh http add urlacl url=http://127.0.0.1:8888/nancy/ user=Everyone
```