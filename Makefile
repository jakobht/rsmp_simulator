DESKTOP = `xdg-user-dir DESKTOP`

define link1
[RSMPGS1]\nName=RSMPGS1\nComment=RSMPGS Road side simulator\nExec=mono /opt/RSMPGS1/RSMPGS1.exe\nType=Application\nTerminal=false\nIcon=\nNoDisplay=false
endef

define link2
[RSMPGS2]\nName=RSMPGS2\nComment=RSMPGS SCADA simulator\nExec=mono /opt/RSMPGS2/RSMPGS2.exe\nType=Application\nTerminal=false\nIcon=\nNoDisplay=false
endef

RSMPGS:	RSMPGS1.exe RSMPGS2.exe

RSMPGS1.exe: Makefile
	nuget restore RSMPGS1/RSMPGS1.sln
	msbuild RSMPGS1/RSMPGS1.sln -p:Configuration=Release

RSMPGS2.exe: Makefile
	nuget restore RSMPGS2/RSMPGS2.sln
	msbuild RSMPGS2/RSMPGS2.sln -p:Configuration=Release

uninstall:
	rm -rf $(HOME)/.config/RSMPGS1
	rm -rf $(HOME)/.config/RSMPGS2
	sudo rm -rf /opt/RSMPGS1
	sudo rm -rf /opt/RSMPGS2

install:
	mkdir -p $(HOME)/.config/RSMPGS1
	mkdir -p $(HOME)/.config/RSMPGS1/LogFiles
	cp -r RSMPGS1/Objects $(HOME)/.config/RSMPGS1
	cp -r RSMPGS1/Settings $(HOME)/.config/RSMPGS1

	mkdir -p $(HOME)/.config/RSMPGS2
	mkdir -p $(HOME)/.config/RSMPGS2/LogFiles
	cp -r RSMPGS2/Objects $(HOME)/.config/RSMPGS2
	cp -r RSMPGS2/Settings $(HOME)/.config/RSMPGS2

	sudo mkdir -p /opt/RSMPGS1
	sudo mkdir -p /opt/RSMPGS2
	sudo cp -r RSMPGS1/bin/Release/* /opt/RSMPGS1
	sudo cp -r RSMPGS2/bin/Release/* /opt/RSMPGS2
	sudo chmod +x /opt/RSMPGS1/RSMPGS1.exe
	sudo chmod +x /opt/RSMPGS2/RSMPGS2.exe

	echo "$(link1)">"$(DESKTOP)"/RSMPGS1.desktop
	chmod +x $(DESKTOP)/RSMPGS1.desktop

	echo "$(link2)">"$(DESKTOP)"/RSMPGS2.desktop
	chmod +x $(DESKTOP)/RSMPGS2.desktop
