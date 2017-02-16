
//js获取URL参数
function getQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return null;
}

$(function () {
    $(document).on({
        ajaxStart: function () {
            //开启读取动画
            $("#loading_ani").removeClass("hidden");
        },
        ajaxStop: function () {
            $("#loading_ani").addClass("hidden");
        }
    });

    //处理数字域
    $(".numeric").numericInput({ allowFloat: true, allowNegative: true });

    //处理日期时间控件
    $(".datetimepicker").datetimepicker({
        todayHighlight: true,
        format: 'yyyy-mm-dd hh:ii',
        autoclose: true,
        todayBtn: true,
        //pickerPosition: "bottom-left",
        language: "zh-CN"
    });

    //日期控件
    $('.datepicker').datetimepicker({
        todayHighlight: true,
        minView: 2,
        format: 'yyyy-mm-dd',
        autoclose: true,
        todayBtn: true,
        //pickerPosition: "top-left",
        language: "zh-CN"
    });

    //表单提交统一处理
    $('#EditForm').validator().on('submit', function (e) {
        if (e.isDefaultPrevented()) {
            //js表单验证不通过
        } else {
            //防止提交表单数据，改用Ajax提交，地址为form里面定义的路径
            e.preventDefault();
            e.defaultPrevented();
            $('#EditForm').ajaxSubmit({
                type: 'post',
                success: function (json) {
                    //var json = eval("(" + data + ")");//转换为json对象（如果用JsonResult返回的话则不需要手动转换）
                    msg(json.message);
                    if (json.success) {
                        setTimeout('location.href = "List"', 1500);
                    }
                },
                error: function () {
                    alert('出错了，请检查网络');
                }
            });
        }
    });

    var terms = $.trim($("#search").val()).split(' ');//获取关键字列表

    //遍历grid里面的数据列，操作列除外
    $("#Content_Body_GridView1 td.td-text").each(function () {
        var temp = $(this).text();
        //构造正则表达式，忽略大小写
        var reg = "/" + terms.join("|") + "/ig";
        //利用js replace函数结合正则表达式和回调函数，把匹配到的字符串先用特殊符号分隔开
        temp = temp.replace(eval(reg), function (str) { return "<span style='color:red'>" + str + "</span>" });
        $(this).html(temp);
    });

});

//简易的弹窗
function msg(content) {
    BootstrapDialog.alert({
        title: '系统提示',
        draggable: true,
        message: content,
        buttons: [{
            label: '确认',
            action: function (dialog) {
                dialog.close();
            }
        }]
    });
}

//搜索
function doSearch() {
    var criteria = $("#search").val();
    location.href = "?s=" + criteria;
}

//放弃修改并刷新的确认
function abandon() {
    BootstrapDialog.show({
        title: '系统提示',
        message: '您确定要放弃修改么？',
        buttons: [{
            label: '确认',
            action: function (dialog) {
                $('#EditForm').resetForm();
                dialog.close();
            }
        }, {
            label: '取消',
            action: function (dialog) {
                dialog.close();
            }
        }]
    });
}

//ajax统一的错误处理方法
function errorHandler(data, errMsg, ex) {
    var err = eval("(" + data.responseText + ")");
    if (err != null) {
        alert(ex + ": " + err.Message);
    } else {
        alert(ex + ": " + errMsg);
    }
}

//删除确认
function delConfirm(id) {
    var HTML = "<div class='box no-border' style='width:100%;height:100%;'>";
    HTML += '您确定要删除ID为<span style="color:red"> ' + id + ' </span>的记录吗?<br/>请注意，记录一经删除则不可恢复！';
    HTML += '<div id="loading" class="overlay hidden"> <i class="fa fa-refresh fa-spin"></i></div>';
    HTML += "</div>";

    BootstrapDialog.show({
        title: '系统提示',
        message: $(HTML),
        buttons: [{
            label: '确认',
            action: function (dialog) {
                $("#loading").removeClass("hidden");
                //提交到本页面删除数据
                $.ajax({
                    async: false,//异步
                    type: "get",
                    url: "Del",
                    data: { 'id': id },
                    success: function (json) {
                        $("#loading").addClass("hidden");
                        //var json = eval("(" + data + ")");//转换为json对象
                        msg(json.message);
                    },
                    error: function () {
                        $("#loading").addClass("hidden");
                        alert('出错了，请检查网络');
                    }
                });
            }
        }, {
            label: '取消',
            action: function (dialog) {
                dialog.close();
            }
        }]
    });
}
