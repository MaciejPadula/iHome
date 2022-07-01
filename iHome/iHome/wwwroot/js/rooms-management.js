$(document).ready(() => {
    const addRoomModal = new bootstrap.Modal('#addRoomModal', {
        keyboard: false
    });
    const addRoomModalEl = document.getElementById('addRoomModal');
    addRoomModalEl.addEventListener('show.bs.modal', event => {
        $("#roomName").removeClass("input-error");
        $("#roomName").val("");
        $("#roomDescription").val("");
        $("#inputErrorRoomName").css("display", "none");
    });

    const renameDeviceModal = new bootstrap.Modal('#renameDeviceModal', {
        keyboard: false
    });
    const renameDeviceModalEl = document.getElementById('renameDeviceModal');
    renameDeviceModalEl.addEventListener('show.bs.modal', event => {
        $("#deviceNameToRename").removeClass("input-error");
        $("#deviceNameToRename").val("");
        $("#inputErrorDeviceName").css("display", "none");
    });

    loadRooms();
    $("#addRoomButton").click(() => {
        if ($("#roomName").val().length >= 3) {
            
            addRoomModal.hide();
            addRoom($("#roomName").val(), $("#roomDescription").val(), "");
        }
        else {
            $("#roomName").addClass("input-error");
            $("#inputErrorRoomName").css("display", "block");
        }
    });
    $("#renameDeviceButton").click(() => {
        if ($("#deviceNameToRename").val().length >= 3) {
            renameDeviceModal.hide();
            renameDevice($("#deviceIdToRename").val(), $("#deviceNameToRename").val());
            //addRoom($("#roomName").val(), $("#roomDescription").val(), "");
        }
        else {
            $("#deviceNameToRename").addClass("input-error");
            $("#inputErrorDeviceName").css("display", "block");
        }
        
    });

    $("#scanNetworkButton").click(() => {
        let ip = $("#octet1").val();
        ip += ".";
        ip += $("#octet2").val();
        ip += ".";
        ip += $("#octet3").val();
        ip += ".";
        for (let i = 1; i < 255; ++i) {
            $.ajax({
                contentType: "application/json",
                dataType: "json",
                type: 'GET',
                url: "http://" + ip + i + '/register'
            }).done((data) => {
                console.log(data);
            });
        }
    });
});

function renameDevice(deviceId, deviceName) {
    $.ajax({
        contentType: "application/json",
        dataType: "json",
        data: JSON.stringify({
            deviceId: deviceId,
            deviceName: deviceName
        }),
        processData: true,
        type: 'POST',
        url: '/api/rooms/renamedevice'
    }).done((data) => {
        loadRooms();
    });
}

function addRoom(name, description, image) {
    const data =
    {
        "roomName": name,
        "roomDescription": description,
        "roomImage": image
    }
    $.ajax({
        contentType: "application/json",
        dataType: "json",
        data: JSON.stringify(data),
        processData: true,
        type: 'POST',
        url: '/api/rooms/addroom'
    }).done((data) => {
        loadRooms();
    });
}
function loadRooms() {
    $("#rooms-container").html("");
    $("#waiting").css("display", "flex");
    $.get("/api/rooms/getrooms/", (data) => {
        $("#rooms-container").html("");
        for (var i in data) {
            $("#rooms-container").append(
                getRoomCard(
                    data[i].roomId,
                    data[i].roomName,
                    data[i].roomDescription,
                    data[i].roomImage,
                    data[i].devices
                )
            );
        }
        $("#waiting").css("display", "none");
    });
}
function removeRoom(ev) {
    if (confirm("Do you want to remove room?")) {
        $.ajax({
            type: 'POST',
            contentType: "application/json",
            dataType: "json",
            url: "/api/rooms/removeroom/" + ev.currentTarget.dataset.roomid,
            success: (data) => {
                loadRooms();
            }
        });
    }
}

function saveRgbLamp(ev) {
    let color="#000";
    let state=0;
    var form = document.getElementById(ev.target.dataset.deviceid).elements;
    for (var i = 0; i < form.length; ++i) {
        if (form[i].name == "color") {
            color = form[i].value;
        }
        else if (form[i].name == "state") {
            if (form[i].checked)
                state = 1;
        }
    }
    color = hexToRgb(color);
    $.ajax({
        contentType: "application/json",
        dataType: "json",
        data: JSON.stringify({
            "deviceId": ev.target.dataset.deviceid,
            "deviceData": JSON.stringify({
                "Red": color.r,
                "Green": color.g,
                "Blue": color.b,
                "State": state
            })
        }),
        processData: true,
        type: 'POST',
        url: '/api/rooms/SetDeviceData'
    });
}

function getData(deviceId, callback) {
    $.ajax({
        contentType: "application/json",
        processData: true,
        type: 'POST',
        url: '/api/rooms/GetDeviceData/' + deviceId,
    }).done(callback);
}