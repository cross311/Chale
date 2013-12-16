
install templates

## Self Hosting

Just runing Nancy I get an error: Nancy.Hosting.Self.AutomaticUrlReservationCreationFailureException

Must run netsh commands in As Administrator cmd.exe
```
netsh http add urlacl url=http://+:8080/nancy/ user=Everyone
netsh http add urlacl url=http://127.0.0.1:8080/nancy/ user=Everyone
```

Getting a normal view to show up is killing me!  if you self host, be prepared to be annoyed about getting your views to render.
The reason why it sucks, is that nancy looks for the views folder from where the exe executes.  If you know anything about .net projects you know the dlls are put in a subfolder of the project.  This subfolder does nto have the views.

Must create a IRootPathProvider and a DefaultNancyBootstrapper just to get teh debug to work.

Razor, hhahahah https://groups.google.com/forum/#!topic/nancy-web-framework/afXvZNND7wI

Been about 1.5 hours to get a simple view and masterpage. grr

## IIS

So far my choice of poison.