function getRoomCard(id, name, description, image, devices) {
    let roomCard = new CardBuilder('room')
        .withAttribute('data-roomid', id)
        .withEventListener('drop', (ev) => {
            ev.preventDefault();
            const deviceId = ev.dataTransfer.getData('deviceId');
            const roomId = ev.currentTarget.dataset.roomid;
            $.ajax({
                contentType: "application/json",
                data: JSON.stringify({
                    "deviceId": deviceId,
                    "roomId": parseInt(roomId)
                }),
                processData: true,
                type: 'POST',
                url: '/api/Rooms/SetDeviceRoom/'
            }).done((data) => {
                loadRooms();
            });
        })
        .withEventListener('dragover', (ev) => {
            ev.preventDefault();
        })
        .withTitle(name)
        .withDescription(description);

    if (image != "null" && image != "" && image != undefined) {
        roomCard.withImage(image);
    }

    let deviceList = document.createElement("div");
    deviceList.className = "room-devices";
    devices.forEach(device => {
        deviceList.append(getDeviceCard(device));
    });
    deviceList.append(getNewDeviceButton(id));

    roomCard
        .addToBody(deviceList)
        .addToBody(new ElementBuilder('button')
            .withClassName('btn remove-button rounded-0')
            .withAttribute('data-roomid', id)
            .withInnerHTML(removeIcon())
            .withEventListener('click', removeRoom)
            .build()
        );
    return roomCard.build();
}

function getDeviceCard(device) {
    let deviceCard = new CardBuilder('device')
        .withDraggable(true)
        .withAttribute('data-deviceid', device.deviceId)
        .withEventListener('dragend', (ev) => {
            ev.target.style.display = "inline-block";
        })
        .withEventListener('dragstart', (ev) => {
            ev.dataTransfer.setData('deviceId', ev.target.dataset.deviceid);
            setTimeout(() => ev.target.style.display = 'none', 0);
        })
        .withEventListener('dragenter', (ev) => {
            ev.dataTransfer.dropEffect = 'move';
        })
        .withTitle(device.deviceName)
        .addToBody(deviceControls(device));

    let renameDeviceButton = new ElementBuilder('button')
        .withClassName('btn btn-sm rounded-0')
        .withInnerHTML(editIcon())
        .withEventListener('click', (ev) => {
            $('#deviceIdToRename').val(device.deviceId);
            $('#deviceNameToRename').val(device.deviceName);
        })
        .withAttribute('data-bs-toggle', 'modal')
        .withAttribute('data-bs-target', '#renameDeviceModal')
        .withStyle('position', 'absolute')
        .withStyle('left', '0')
        .build();

    deviceCard
        .addToCard(renameDeviceButton)
        .addToCard(new ImageBuilder()
            .withClassName('device-image')
            .withDraggable(false)
            .withSrc(getDeviceImageUrl(device))
            .build()
        );
    return deviceCard.build();
}

function deviceControls(device) {
    let dataForm = new ElementBuilder('form')
        .withId(device.deviceId)
        .withAttribute('data-deviceid', device.deviceId)
        .build();

    const data = JSON.parse(device.deviceData);
    if (device.deviceType == 1) {
        let colorInput = new InputBuilder()
            .withType('color')
            .withName('color')
            .withAttribute('data-deviceid', device.deviceId)
            .withValue(rgbToHex(data.Red, data.Green, data.Blue))
            .withEventListener('change', saveRgbLamp)
            .build();

        let stateBody = document.createElement("div");
        stateBody.className = "form-check form-switch";

        let stateButton = new InputBuilder()
            .withClassName('form-check-input')
            .withType('checkbox')
            .withName('state')
            .withId("id1")
            .withAttribute('data-deviceid', device.deviceId)
            .withRole('switch')
            .withEventListener('change', saveRgbLamp)
            .build();

        if (data.State == 1) {
            stateButton.checked = true;
        }

        let label = document.createElement("label");
        label.className = "label";
        label.for = "id1";
        label.innerHTML = "Device State:";

        stateBody.append(stateButton);
        stateBody.append(label);

        dataForm.append(colorInput);
        dataForm.append(stateBody);
    }
    else if (device.deviceType == 2) {
        let temperatureRead = document.createElement("div");
        temperatureRead.className = "temperature";
        temperatureRead.style.width = "100%";
        temperatureRead.style.textAlign = "left";
        temperatureRead.innerHTML = "Temperature: 0&#8451;";

        let pressureRead = document.createElement("div");
        pressureRead.className = "pressure";
        pressureRead.style.width = "100%";
        pressureRead.style.textAlign = "left";
        pressureRead.innerHTML = "Pressure: 0 hPa";

        
        dataForm.append(temperatureRead);
        dataForm.append(pressureRead);
        setInterval(() => {
            getData(device.deviceId, (data) => {
                //console.log(JSON.parse(data).celsius)
                temperatureRead.innerHTML = "Temperature: " + JSON.parse(data).celsius + "&#8451;";
                pressureRead.innerHTML = "Pressure: " + JSON.parse(data).pressure + " hPa";
            });
        },2500);
    }

    return dataForm;
}

function getDeviceImageUrl(device) {
    if (device.deviceType == 1) {
        return "../resources/images/rgbLamp.png";
    }
    else if (device.deviceType == 2) {
        return "../resources/images/temperature.png";
    }
    else if (device.deviceType == 3) {
        return "../resources/images/pir.png";
    }

    return "";
}

function getNewDeviceButton(roomId) {
    let card = new CardBuilder('device').withClassName('card device-card new-device-card');
    card.addToBody(new ElementBuilder('div')
            .withClassName('new-device-button-icon')
            .withInnerHTML(createIcon())
            .build()
        )
        .withAttribute('data-bs-toggle', 'modal')
        .withAttribute('data-bs-target', '#setupDeviceModal')
        .withEventListener('click', (ev) => {
            $('#room-id-input').val(roomId);
        });
    return card.build();
}

function createNewDevice(deviceInfo) {
    let main = document.createElement('div');
    main.className = 'new-device-info';

    let deviceId = document.createElement('div');
    deviceId.className = 'new-device-address';
    deviceId.innerText = deviceInfo.deviceId;

    let deviceImgContainer = document.createElement('div');
    deviceImgContainer.className = 'new-device-image-container'
    let deviceImage = document.createElement('img');
    deviceImage.src = getDeviceImageUrl(deviceInfo);
    deviceImgContainer.append(deviceImage);

    let deviceNameInput = document.createElement('input');
    deviceNameInput.type = 'text';
    deviceNameInput.style.display = 'none';
    deviceNameInput.style.transition = '1s';

    let button = document.createElement('button');
    button.addEventListener('click', (ev) => {
        if (deviceNameInput.style.display == 'none') {
            deviceNameInput.style.display = 'block';
        }
        else if (deviceNameInput.value == '') {
            deviceNameInput.style.display = 'none';
        }
        else {
            let data = {
                'deviceId': deviceInfo.deviceId,
                'deviceType': deviceInfo.deviceType,
                'deviceName': deviceNameInput.value,
                'roomId': $('#room-id-input').val()
            };
            if (deviceInfo.deviceType == 1) {
                data['deviceData'] = '{"Red":255, "Green":255, "Blue":255, "State":1}';
            }
            $.ajax({
                contentType: "application/json",
                dataType: "json",
                data: JSON.stringify(data),
                type: 'POST',
                url: '/api/rooms/adddevice/' + deviceInfo.id
            }).done((data) => {
                window.location.reload(true);
            });
        }
    });

    main.append(deviceNameInput);
    main.append(deviceImgContainer);
    main.append(deviceId);
    main.append(button);
    
    return main;
}

