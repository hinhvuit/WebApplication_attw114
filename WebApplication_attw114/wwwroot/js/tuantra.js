// Externalized script for Views/OndutyFit/Tuantra.cshtml
// Requires: jQuery loaded before this file
(function () {
    'use strict';

    $(function () {
        // wire events
        $('#txt_EmpNo').on('change', function () {
            if ($('#txt_EmpNo').val() !== '') {
                getListTest();
            }
            gettit();
        });

        $('#ddl_TypePatrol').on('change', function () {
            if ($('#txt_EmpNo').val() !== '') {
                getListTest();
            }
            gettit();
        });

        gettit();

        var lati = $('#hid_Lati').val();
        var longti = $('#hid_Long').val();
        var er = $('#hid_Error').val();
        var ms = $('#hid_Mess').val();
        var RederectUrl = $('#RedirectToSuccess').val();
        console.log('Vị trí của bạn là: ' + lati + ', ' + longti);
        if (!(lati === '0' && longti === '0')) {
            $('#btn_UpdateLocation').hide();
            if (er == "1") { alert(ms); location.href = RederectUrl; }
        }

        $('#btn_UpdateLocation').on('click', function () {
            console.log('startupdatelocation');
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
                    console.log('updatelocation Success:', data);
                    window.close();
                },
                error: function (xhr, status, error) {
                    console.error('updatelocation Error:', error);
                    alert('Cập Nhật Vị Trí không thành công');
                }
            });
        });

        $('#btnSign').on('click', function () {
            var listRule = [];
            var empNo = $('#txt_EmpNo').val();
            var empName = $('#txt_EmpName').val();
            var typePatrol = $('#ddl_TypePatrol').val();
            var codePoint = $('#hid_LocationID').val();
            var latiNew = $('#hid_Latinew').val();
            var longtiNew = $('#hid_Longtinew').val();

            if (!empNo) {
                alert('Vui lòng nhập Employee No.');
                return;
            }
            if (!empName) {
                alert('Vui lòng nhập Employee Name.');
                return;
            }
            if (!codePoint) {
                alert('Vui lòng chọn Location.');
                return;
            }

            $('input[type="radio"]:checked').each(function () {
                var name = $(this).attr('name') || '';
                var parts = name.split('_');
                var ruleID = parts[1] || '';
                var value = $(this).val();
                var memo = $('input[name="memo_' + ruleID + '"]').val();
                var isOk = value === 'OK';

                listRule.push({
                    ruleID: parseInt(ruleID, 10),
                    isOk: isOk,
                    memo: memo || '',
                    imageName: ''
                });
            });

            if (listRule.length === 0) {
                alert('Vui lòng chọn ít nhất một rule.');
                return;
            }

            $.ajax({
                type: 'POST',
                url: 'SignPatrol',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({
                    empNo: empNo,
                    typePatrol: parseInt(typePatrol, 10),
                    codePoint: codePoint,
                    empName: empName,
                    lati: parseFloat(latiNew),
                    longti: parseFloat(longtiNew),
                    listChecked: listRule
                }),
                success: function (data) {
                    alert('Ký thành công');
                    getListTest();
                    console.log('SignPatrol Success:', data);
                },
                error: function (xhr, status, error) {
                    console.error('SignPatrol Error:', error);
                    alert('Ký không thành công');
                }
            });
        });
    });

    function gettit() {
        if ($('#ddl_TypePatrol').val() === '2') {
            $('#lbl_tit').text('Tuần Tra Của Nhân Viên An Ninh');
        } else {
            $('#lbl_tit').text('Tuần Tra Của Nhân Viên Bảo Vệ');
        }
    }

    function getListTest() {
        var codepoint = $('#hid_LocationID').val();
        var empNo = $('#txt_EmpNo').val();
        var typePatrol = $('#ddl_TypePatrol').val();
        var parameter = {
            code_point: codepoint,
            empNo: empNo,
            typePatrol: typePatrol
        };
        $.ajax({
            type: 'POST',
            dataType: 'json',
            contentType: 'application/json',
            data: JSON.stringify(parameter),
            url: 'GetList',
            success: function (dataOut) {
                if (dataOut && dataOut.list) {
                    renderHistory(dataOut.list);
                } else {
                    renderHistory([]);
                }
            },
            error: function () {
                alert('error get List history Sign');
            }
        });

        $.ajax({
            url: 'GetRules',
            type: 'GET',
            data: {
                codePoint: codepoint,
                typePatrol: typePatrol
            },
            success: function (response) {
                console.log('Success:', response);
                $('#body_rule').empty();
                $.each(response, function (index, item) {
                    var row = '<tr>' +
                        '<td>' + (item.ruleName || '') +
                        '<input type="hidden" name="ruleID_' + item.id + '" value="' + item.id + '" />' +
                        '</td>' +
                        '<td><input type="radio" name="rule_' + item.id + '" value="OK" checked="checked" /> ✓</td>' +
                        '<td><input type="radio" name="rule_' + item.id + '" value="NG" /> ✗</td>' +
                        '<td><input type="text" name="memo_' + item.id + '" class="form-control" /></td>' +
                        '</tr>';
                    $('#body_rule').append(row);
                });
            },
            error: function (xhr, status, error) {
                console.error('Error:', error);
                if (xhr.status === 400) {
                    console.error('Bad Request:', xhr.responseText);
                }
            }
        });
    }

    function renderHistory(list) {
        var html = '';
        if (!Array.isArray(list) || list.length === 0) {
            html = "<tr><td colspan='5'>Không có dữ liệu tuần tra</td></tr>";
            $('#tblHistory tbody').html(html);
            return;
        }

        html = '';
        var canvas = document.getElementById('mycanvas');
        var contextt = canvas ? canvas.getContext('2d') : null;
        if (contextt) {
            // optional: clear canvas before redraw
            // contextt.clearRect(0, 0, canvas.width, canvas.height);
        }

        for (var i = 0; i < list.length; i++) {
            var item = list[i];
            html += '<tr>' +
                '<td>' + (item.namePoint || '') + '</td>' +
                '<td>' + (item.statusSign || '') + '</td>' +
                '<td>' + (item.statusPlace || '') + '</td>' +
                '<td>' + (item.memo || '') + '</td>' +
                '<td>' + (item.workDate || '') + '</td>' +
                '</tr>';

            if (contextt && typeof item.x !== 'undefined' && typeof item.y !== 'undefined') {
                var status1 = item.statusSign || '';
                var color = (status1 === 'da_ky' || status1 === 'Đã Ký') ? '#00FF00' : '#FF0000';
                var centerX = item.x;
                var centerY = item.y;
                var radius = 12;
                contextt.beginPath();
                contextt.fillStyle = color;
                contextt.strokeStyle = color;
                contextt.lineWidth = 2;
                contextt.arc(centerX, centerY, radius, 0, 2 * Math.PI, false);
                contextt.fill();
                contextt.stroke();
            }
        }
        $('#tblHistory tbody').html(html);
    }

})();