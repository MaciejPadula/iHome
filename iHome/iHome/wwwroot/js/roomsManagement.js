

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
});

function addRoom(name, description, image) {
    const uuid = $("#uuid").val();
    const dane =
    {
        "name": name,
        "description": description,
        "image": image,
        "uuid": uuid
    }
    
    $.ajax({
        contentType: "application/json",
        dataType: "json",
        data: JSON.stringify(dane),
        processData: true,
        type: 'POST',
        url: '/api/rooms/addroom'
    }).done((data) => {
        loadRooms();
    });
    

}
function loadRooms() {
    const uuid = $("#uuid").val();
    $("#rooms-container").html("");
    $("#waiting").css("display", "flex");
    $.get("/api/rooms/getrooms/" + uuid, (data) => {
        $("#rooms-container").html("");
        for (var i in data) {
            $("#rooms-container").append(getRoomCard(data[i].id, data[i].name, data[i].description, data[i].image, uuid));
            getDevices(data[i].id);
        }
        $("#waiting").css("display", "none");
    });
    
}
function removeRoom(id) {
    const uuid = $("#uuid").val();
    if (confirm("Do you want to remove room?")) {
        $.ajax({
            type: 'POST',
            dataType: "json",
            url: "/api/rooms/removeroom/" + id,
            success: (data) => {
                loadRooms();
            }
        });
    }
}


function getRoomCard(id, name, description, image) {
    const uuid = $("#uuid").val();
    content = "";
    function applyContent(apply) {
        content += apply;
    }
    content += "<img src=\"" + image + "\" class=\"card-img-top\" alt=\"...\">";
    content += "<div class=\"card-body\">";
    content += "<h5 class=\"card-title\">" + name + "</h5>";
    content += "<p class=\"card-text\">" + description + "</p>";
    content += "<input type=\"button\" class=\"btn btn-primary remove-button\" value=\"Remove\" onClick=\"removeRoom(" + id + ",'" + uuid + "')\"/>";
    content += "<div id=\"room-devices\">";
    content += "</div>";
    content += "</div>";
    return "<div class=\"card room-card\" id=\""+ id +"\">" + content + "</div>";
}

function getDevices(roomId) {
    const uuid = $("#uuid").val();
    $.get("/api/rooms/GetDevices/" + roomId, (data) => {
        for (var device in data) {
            $("#rooms-container #" + roomId +" #room-devices").append(getDeviceCard(data[device]));
        }
    });
}

function getDeviceCard(device) {

    const rgbFunction = "changeColor(hexToRgb(this.value).r, hexToRgb(this.value).g, hexToRgb(this.value).b)";

    let content = "";
    const data = JSON.parse(device.data);
    console.log(data.Red);
    //content += "<img src=\"" + image + "\" class=\"card-img-top\" alt=\"...\">";
    content += "<div class=\"card-body\">";
    content += "<h5 class=\"card-title\">" + device.name + "</h5>";
    //content += "<input type=\"button\" class=\"btn btn-primary remove-button\" value=\"Remove\" onClick=\"removeRoom(" + id + ",'" + uuid + "')\"/>";

    if (device.type == 1) {
        content += "<input type=\"color\" name=\"" + device.deviceId + "\" value=\"" + rgbToHex(data.Red, data.Green, data.Blue) + "\" onChange=\"" + rgbFunction+"\"/>";
    }
    content += "</div>";
    return "<div class=\"card device-card\" id=\"" + device.deviceId + "\">" + content + "</div>";
}

function componentToHex(c) {
    var hex = c.toString(16);
    return hex.length == 1 ? "0" + hex : hex;
}

function rgbToHex(r, g, b) {
    return "#" + componentToHex(r) + componentToHex(g) + componentToHex(b);
}

function hexToRgb(hex) {
    var result = /^#?([a-f\d]{2})([a-f\d]{2})([a-f\d]{2})$/i.exec(hex);
    return result ? {
        r: parseInt(result[1], 16),
        g: parseInt(result[2], 16),
        b: parseInt(result[3], 16)
    } : null;
}
function changeColor(r, g, b) {
    console.log(r + g + b);
}