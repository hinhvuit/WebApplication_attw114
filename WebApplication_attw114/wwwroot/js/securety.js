// Externalized script for Views/OndutyFit/Securety.cshtml
// Requires jQuery loaded before this file
(function () {
    'use strict';

    function getListTest() {
        var codepoint = $("#hid_LocationID").val();
        var empNo = $("#hid_EmpNo").val();
        if (empNo !== "Na") {
            $.ajax({
                type: 'POST',
                url: 'GetList',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({ code_point: codepoint, empNo: empNo, typePatrol: 3 }),
                success: function (data) {
                    renderHistory(data.list || []);
                },
                error: function () {
                    alert('Lỗi khi lấy lịch sử tuần tra');
                }
            });

            $.ajax({
                url: 'GetRules',
                type: 'GET',
                data: { codePoint: codepoint, typePatrol: 3 },
                success: function (response) {
                    $('#body_rule').empty();
                    $.each(response, function (index, item) {
                        var row = '<tr>' +
                            '<td>' + (item.ruleName || '') + '<input type="hidden" name="ruleID_' + item.id + '" value="' + item.id + '" /></td>' +
                            '<td><input type="radio" name="rule_' + item.id + '" value="OK" checked /></td>' +
                            '<td><input type="radio" name="rule_' + item.id + '" value="NG" /></td>' +
                            '<td><input type="text" name="memo_' + item.id + '" class="form-control" /></td>' +
                            '</tr>';
                        $('#body_rule').append(row);
                    });
                },
                error: function (xhr, status, error) {
                    console.error('GetRules Error:', error);
                }
            });
        } else {
            var usid = $("#hid_UserID").val();
            var lati = $("#hid_Lati").val();
            var longti = $("#hid_Long").val();
            var urlredirect = "https://vn-safety.foxconn.com/swispe/SwipesRegisterZalo?lati=" + lati + "&longti=" + longti + "&userid=" + usid;
            window.location.href = urlredirect;
        }
    }

    function renderHistory(list) {
        var html = "";
        if (!Array.isArray(list) || list.length === 0) {
            html = "<tr><td colspan='5'>Không có dữ liệu tuần tra</td></tr>";
            $("#tblHistory tbody").html(html);
            return;
        }

        var canvas = document.getElementById("mycanvas");
        var ctx = canvas ? canvas.getContext("2d") : null;
        if (ctx) {
            // optional: clear canvas before redraw
            // ctx.clearRect(0, 0, canvas.width, canvas.height);
        }

        list.forEach(function (item) {
            html += '<tr>' +
                '<td>' + (item.namePoint || '') + '</td>' +
                '<td>' + (item.statusSign || '') + '</td>' +
                '<td>' + (item.statusPlace || '') + '</td>' +
                '<td>' + (item.memo || '') + '</td>' +
                '<td>' + (item.workDate || '') + '</td>' +
                '</tr>';

            if (ctx && typeof item.x !== 'undefined' && typeof item.y !== 'undefined') {
                var color = (item.statusSign === "da_ky" || item.statusSign === "Đã Ký") ? "#00FF00" : "#FF0000";
                ctx.beginPath();
                ctx.fillStyle = color;
                ctx.strokeStyle = color;
                ctx.lineWidth = 2;
                ctx.arc(item.x, item.y, 12, 0, 2 * Math.PI);
                ctx.fill();
                ctx.stroke();
            }
        });

        $("#tblHistory tbody").html(html);
    }

    $(function () {
        getListTest();

        var lati = $('#hid_Lati').val();
        var longti = $('#hid_Long').val();
        var er = $('#hid_Error').val();
        var ms = $('#hid_Mess').val();
        var redirectUrl = $("#RedirectToSuccess").val();

        if (lati !== "0" || longti !== "0") {
            $("#btn_UpdateLocation").hide();
            if (er === "1") {
                alert(ms);
                if (redirectUrl) location.href = redirectUrl;
            }
        }

        $("#btn_UpdateLocation").on('click', function () {
            var latiNew = $('#hid_Latinew').val();
            var longtiNew = $('#hid_Longtinew').val();
            var codePoint = $('#hid_LocationID').val();

            $.ajax({
                type: 'POST',
                url: 'UpdateLocation',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({
                    codePoint: codePoint,
                    lati: parseFloat(latiNew),
                    longti: parseFloat(longtiNew)
                }),
                success: function (data) {
                    alert('Cập Nhật Vị Trí Thành Công');
                    window.close();
                },
                error: function (xhr, status, error) {
                    console.error('UpdateLocation Error:', error);
                    alert('Cập Nhật Vị Trí không thành công');
                }
            });
        });

        $('#btnSign').on('click', function () {
            var listRule = [];
            var empNo = $('#hid_EmpNo').val();
            var empName = $('#hid_EmpName').val();
            var codePoint = $('#hid_LocationID').val();
            var latiNew = $('#hid_Latinew').val();
            var longtiNew = $('#hid_Longtinew').val();

            if (!empNo || !empName || !codePoint) {
                alert('Vui lòng nhập đầy đủ thông tin nhân viên và vị trí.');
                return;
            }

            $('input[type="radio"]:checked').each(function () {
                var ruleID = ($(this).attr('name') || '').split('_')[1];
                var value = $(this).val();
                var memo = $('input[name="memo_' + ruleID + '"]').val();

                listRule.push({
                    ruleID: parseInt(ruleID, 10),
                    isOk: value === 'OK',
                    memo: memo || '',
                    imageName: ''
                });
            });

            if (listRule.length === 0) {
                alert('Vui lòng chọn ít nhất một hạng mục.');
                return;
            }

            $.ajax({
                type: 'POST',
                url: 'SignPatrol',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({
                    empNo: empNo,
                    empName: empName,
                    codePoint: codePoint,
                    typePatrol: 3,
                    lati: parseFloat(latiNew),
                    longti: parseFloat(longtiNew),
                    listChecked: listRule
                }),
                success: function (data) {
                    alert('Ký thành công');
                    getListTest();
                },
                error: function (xhr, status, error) {
                    console.error('SignPatrol Error:', error);
                    alert('Ký không thành công');
                }
            });
        });
    });
})();