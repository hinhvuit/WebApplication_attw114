// Externalized script for OndutySignEmpAttw114\Onduty.cshtml
// Expects:
// - jQuery already loaded
// - element #onduty-data present with data-history (JSON array)
// - civetjs 'cm.getGPS' available when used
// - DOM contains the labels/inputs referenced in original view
(function () {
    'use strict';

    function parseHistory() {
        var el = document.getElementById('onduty-data');
        if (!el) return [];
        var raw = el.dataset.history || '[]';
        try {
            return JSON.parse(raw);
        } catch (ex) {
            console.error('Failed to parse history JSON', ex);
            return [];
        }
    }

    function drawPoints(history) {
        var canvas = document.getElementById('mycanvas');
        if (!canvas) return;
        var ctx = canvas.getContext('2d');
        if (!ctx) return;

        history.forEach(function (item) {
            var status1 = String(item.Status);
            var color = (status1 === '1' || status1 === '3') ? '#00FF00' : '#FF0000';
            var centerX = item.X;
            var centerY = item.Y;
            var radius = 12;
            ctx.beginPath();
            ctx.fillStyle = color;
            ctx.strokeStyle = color;
            ctx.lineWidth = 2;
            ctx.arc(centerX, centerY, radius, 0, 2 * Math.PI, false);
            ctx.fill();
            ctx.stroke();
        });
    }

    function getListTest() {
        var LocationIDstr = $("#LocationID").text();
        var EmpNoIDstr = $("#lbl_EmpNoFromIC").text();
        var parameter = {
            LocationID: LocationIDstr,
            EmpNo: EmpNoIDstr
        };
        $.ajax({
            type: 'POST',
            dataType: 'json',
            contentType: 'application/json',
            data: JSON.stringify(parameter),
            url: 'GetHistorySignNow',
            success: function (dataOut) {
                try {
                    var canvas = document.getElementById("mycanvas");
                    var contextt = canvas.getContext("2d");
                    dataOut.forEach(function (itemData) {
                        var status1 = String(itemData.Status);
                        var color = (status1 === "1" || status1 === "3") ? '#00FF00' : '#FF0000';
                        var centerX = itemData.X;
                        var centerY = itemData.Y;
                        var radius = 12;
                        contextt.beginPath();
                        contextt.fillStyle = color;
                        contextt.strokeStyle = color;
                        contextt.lineWidth = 2;
                        contextt.arc(centerX, centerY, radius, 0, 2 * Math.PI, false);
                        contextt.fill();
                        contextt.stroke();
                    });
                } catch (ex) {
                    console.error('draw error', ex);
                }
            },
            error: function () {
                alert('error get List history Sign');
            }
        });
    }

    function UploadImageFile() {
        var retval = '';
        var files = document.getElementById('FileupLoadImage').files;
        if (!files || files.length === 0) return '';
        var fileName = files[0].name || '';
        var fileExtension = fileName.split('.').pop();

        var today = new Date();
        var Nameorigin = today.getFullYear().toString() +
            today.getMonth().toString() +
            today.getDate().toString() +
            today.getHours().toString() +
            today.getMinutes().toString() +
            today.getSeconds().toString() +
            today.getMilliseconds().toString();

        if (window.FormData !== undefined) {
            var data = new FormData();
            for (var x = 0; x < files.length; x++) {
                data.append("file" + x, files[x]);
            }
            data.append("ImageName", Nameorigin.toString());
            $.ajax({
                type: "POST",
                url: 'UploadImageNew1',
                contentType: false,
                processData: false,
                data: data,
                success: function (result) {
                    $("#FileupLoadImage").val(null);
                },
                error: function (xhr, status, exception) {
                    console.error('UploadImage error', exception);
                }
            });
        } else {
            alert("Upgrade your browser!");
        }
        retval = Nameorigin + "." + fileExtension;
        return retval;
    }

    function delay(ms) {
        return new Promise(function (resolve) { setTimeout(resolve, ms); });
    }

    function OnsubMit() {
        var file1 = document.getElementById('FileupLoadImage');
        var ImageName;
        if (file1 && file1.files.length !== 0) {
            ImageName = UploadImageFile();
            // keep original behavior but wait a bit
            return delay(1200).then(function () {
                submitSign(ImageName);
            });
        } else {
            ImageName = "Na";
            submitSign(ImageName);
            return Promise.resolve();
        }
    }

    function submitSign(ImageName) {
        var RederectUrl = $("#RedirectToSuccess").val();
        var Isoke = $("#Select1").val();
        var contentSign = $("#TextArea_SignNote").val();
        if (!contentSign) contentSign = "Na";
        var EmpNostr = $("#lbl_EmpNoFromIC").text();
        var EmpNamestr = $("#lbl_EmpNameFromIC").text();
        var LocationIDstr = $("#LocationID").text();
        var parameter = {
            Type: 1,
            EmpNo: EmpNostr,
            EmpName: EmpNamestr,
            LocationID: LocationIDstr,
            Isoke: Isoke,
            ImageName: ImageName,
            Notes: contentSign
        };
        $.ajax({
            type: 'POST',
            dataType: 'text',
            url: 'SignAttw',
            data: parameter,
            success: function (daa) {
                location.href = RederectUrl;
            },
            error: function (daa, status, exception) {
                alert('An error occurred uploading data.');
                console.error('SignAttw error', exception || status, daa);
            }
        });
    }

    function bindEvents() {
        $("#btn_Upload").on('click', function () {
            var file1 = document.getElementById('FileupLoadImage');
            if (file1 && file1.files.length != 0) {
                var ImageName = UploadImageFile();
                alert('Da upload file:' + ImageName);
            } else {
                alert("Khong co file nao duoc chon");
            }
        });

        $("#Radio1").on('change', function () {
            alert("you have changed value from:" + (window.TypeForm || ''));
            window.TypeForm = 1;
        });

        $("#Radio2").on('change', function () {
            alert("you have changed value from:" + (window.TypeForm || ''));
            window.TypeForm = 2;
        });

        $('#btn_Submit').on('click', function () {
            if (typeof cm !== 'undefined' && typeof cm.getGPS === 'function') {
                cm.getGPS({
                    modal: true,
                    success: function (res) {
                        var Curlat = parseFloat(res.lat);
                        var Curlng = parseFloat(res.lng);
                        var Flatitude = parseFloat($("#lbl_latitude").text().toString());
                        var Flongitude = parseFloat($("#lbl_longitude").text().toString());
                        if (Math.abs(Curlat - Flatitude) <= 0.000139 && Math.abs(Curlng - Flongitude) <= 0.000139) {
                            OnsubMit();
                        } else {
                            alert("vị trí bạn đứng không phù hợp với vị trí tuần tra vui lòng đến đúng vị trí cần tuần tra");
                            OnsubMit();
                        }
                    },
                    error: function (error) {
                        alert("vui lòng bật cho phép chạy javascript trong phần cài đặt hoặc bật định vị để thao tác!");
                        return false;
                    }
                });
            } else {
                // fallback: submit directly
                OnsubMit();
            }
        });

        $("#btn_testLocation").on('click', function () {
            var LocationIDstr = $("#LocationID").text();
            if (typeof cm !== 'undefined' && typeof cm.getGPS === 'function') {
                cm.getGPS({
                    modal: true,
                    success: function (res) {
                        var Curlat = parseFloat(res.lat);
                        var Curlng = parseFloat(res.lng);
                        var parameter = {
                            LocationID: LocationIDstr,
                            Latitude: Curlat,
                            Longitude: Curlng
                        };
                        $.ajax({
                            type: 'POST',
                            dataType: 'text',
                            url: 'UpdateLocation',
                            data: parameter,
                            success: function (daa) {
                                location.href = $("#RedirectToSuccess").val();
                            },
                            error: function (daa, status, exception) {
                                alert('An error occurred uploading data.');
                            }
                        });
                    }
                });
            } else {
                alert('cm.getGPS is not available');
            }
        });

        $('#btn_test').on('click', function () {
            $.ajax({
                type: 'POST',
                dataType: 'json',
                url: 'TestController',
                success: function (daa) {
                    alert('Complete name: ' + daa[0]['name']);
                },
                error: function () {
                    alert('An error occurred uploading data.');
                }
            });
        });
    }

    // DOM ready
    $(function () {
        var history = parseHistory();
        drawPoints(history);
        bindEvents();
        // call existing global CallApi if present
        if (typeof window.CallApi === 'function') {
            try { window.CallApi(); } catch (ex) { console.error(ex); }
        }
    });

})();