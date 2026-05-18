// Externalized script for Views/FireCheck/Index.cshtml
// Requires jQuery loaded before this file
(function () {
    'use strict';

    function getListTest() {
        var codedevice = $("#hid_codedevice").val();
        if (!codedevice) return;
        $.ajax({
            url: '/GetRules',
            type: 'GET',
            data: { codeDevice: codedevice },
            success: function (response) {
                console.log('GetRules Response:', response);
                $('#body_rule').empty();
                $.each(response, function (index, item) {
                    var row = '<tr>' +
                        '<td style="text-align:left;">' + (item.ruleName || '') + '<input type="hidden" name="ruleID_' + item.id + '" value="' + item.id + '" /></td>' +
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
    }

    function renderHistory(list) {
        var html = "";
        if (!Array.isArray(list) || list.length === 0) {
            html = "<tr><td colspan='5'>Không có dữ liệu tuần tra</td></tr>";
        } else {
            var canvas = document.getElementById("mycanvas");
            var contextt = canvas ? canvas.getContext("2d") : null;
            if (contextt) {
                // contextt.clearRect(0, 0, canvas.width, canvas.height);
            }
            list.forEach(function (item) {
                html += "<tr>" +
                    "<td>" + (item.ruleName || '') + "</td>" +
                    "<td>" + (item.statusSign || '') + "</td>" +
                    "<td>" + (item.statusPlace || '') + "</td>" +
                    "<td>" + (item.memo || '') + "</td>" +
                    "<td>" + (item.workDate || '') + "</td>" +
                    "</tr>";

                if (contextt && typeof item.x !== 'undefined' && typeof item.y !== 'undefined') {
                    var status1 = item.statusSign;
                    var color = (status1 == "da_ky" || status1 == "Đã Ký") ? '#00FF00' : '#FF0000';
                    contextt.beginPath();
                    contextt.fillStyle = color;
                    contextt.strokeStyle = color;
                    contextt.lineWidth = 2;
                    contextt.arc(item.x, item.y, 12, 0, 2 * Math.PI, false);
                    contextt.fill();
                    contextt.stroke();
                }
            });
        }
        $("#tblHistory tbody").html(html);
    }

    $(function () {
        getListTest();

        $('#btnSign').on('click', function () {
            var listRule = [];
            var userID = $('#hid_UserID').val();
            var deviceID = $('#hid_dvid').val();
            var memo = $('#txt_Memo').val();
            var redirectUrl = $("#RedirectToSuccess").val();

            if (!userID || !deviceID) {
                alert('Vui lòng nhập đầy đủ thông tin nhân viên');
                return;
            }

            $('input[type="radio"]:checked').each(function () {
                var ruleID = ($(this).attr('name') || '').split('_')[1];
                var value = $(this).val();
                var ruleMemo = $('input[name="memo_' + ruleID + '"]').val();
                listRule.push({
                    ruleID: parseInt(ruleID, 10),
                    isOk: value === 'OK',
                    memo: ruleMemo || ''
                });
            });

            if (listRule.length === 0) {
                alert('Vui lòng chọn ít nhất một hạng mục.');
                return;
            }

            $.ajax({
                type: 'POST',
                url: 'SaveCheckedList',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({
                    userID: userID,
                    deviceID: deviceID,
                    memo: memo,
                    listChecked: listRule
                }),
                success: function (data) {
                    alert('Ký thành công');
                    if (redirectUrl) window.location.href = redirectUrl;
                },
                error: function (xhr, status, error) {
                    console.error('SignPatrol Error:', xhr.responseText || error);
                    alert('Ký không thành công: ' + (xhr.responseText || error));
                }
            });
        });
    });
})();