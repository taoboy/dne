/*
//生成menuJs的SQL：
select 
'{ name : " '+cast(p.value as varchar(50))+'", icon: "fa-database", url:"../_'+t.name+'/List" } ,'
from sys.tables as t
left join sys.extended_properties as p 
on t.object_id = p.major_id and p.minor_id = 0
*/

function makeMenu(menu) {
    var HTML = '';
    HTML += '<li class="treeview active">\n';
    HTML += '<a href="#" class="bg-purple">\n';
    HTML += '<i class="fa ' + menu.icon + '"></i>\n';
    HTML += '<span>' + menu.name + '</span>\n';
    HTML += '</a>\n';
    HTML += '<ul class="treeview-menu">\n';

    for (var i = 0; i < menu.menus.length; i++) {
        var item = menu.menus[i];
        HTML += '<li><a href="' + item.url + '"><i class="fa ' + item.icon + '"></i> ' + item.name + '</a></li>\n';
    }

    HTML += '</ul>\n';
    HTML += '</li>\n';

    return HTML;
}

$(function () {
    //处理侧栏菜单项
    var menu = $(".sidebar-menu")[0];
    var HTML = "";
    for (var i = 0; i < menus.length; i++)
        HTML += makeMenu(menus[i]);

    $(menu).prepend(HTML);
});

var menus = [

        {
            icon: "fa-star",
            name: "常用功能",
            menus: [
                { name: " 发表文章", icon: "fa-pencil", url: "../Post/Edit" },
                { name: " 新增类别", icon: "fa-file-o", url: "../Category/Edit" },
                { name: " 新增用户", icon: "fa-user-plus", url: "../Users/Edit" },
                { name: " 查看首页", icon: "fa-home", url: "../index/Home" }
            ]
        },
        {
            icon: "fa-gears",
            name: "基础数据管理",
            menus: [
                { name: " 分类管理", icon: "fa-gear", url: "../Category/List" },
                { name: " 文章管理", icon: "fa-gear", url: "../Post/List" },
                { name: " 用户管理", icon: "fa-gear", url: "../Users/List" }
            ]
        }
    ];


