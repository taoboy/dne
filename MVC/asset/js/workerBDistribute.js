$(function () {
    var name = localStorage.getItem('name');
    var role = localStorage.getItem('role');
    var roleid = localStorage.getItem('roleid');
    $('header .dropdown').find('img').attr('src', '/task/asset/img/user' + roleid + '.jpg');

	var formCon = $('.main').find('.form-content');//获取到各个任务分派内容到formCon
	var itemIndex = 0;//itemIndex记录当前选中的任务属于第几个任务
	var nodeArr = new Array();//nodeArr记录所有的任务的分派内容
	init();
	$('.main').find('.form-select').on('change', function () {//任务选择改变时
		dataDeal('.main','.form-content','select','set');
		dataDeal('.main','.form-content','textarea','set');
		nodeArr[itemIndex] = $('.main').find('.form-content').clone();//更新当前的分派内容
		$('.main').find('.form-content').remove();//删除节点
		var option = $(this).find('option');//获取到每个任务选项到option
		var val = $(this).val();//记录当前选中的任务到val
		option.each(function(i) {//此处得到itemIndex
			if(option[i].innerHTML == val){
				itemIndex = i;
				return false;
			}
		});
		$('.pass-task').before(nodeArr[itemIndex]);//从nodeArr中获取分派内容插入到确定按钮区域之前
		dataDeal('.main','.form-content','textarea','get');
		dataDeal('.main','.form-content','select','get');
		form();
	});
	function init(){//初始化
		formCon.each(function(i){//克隆当前的任务分派情况到formCon中保存
			nodeArr[i] = $(formCon[i]).clone();
			if(i>0){
				$(formCon[i]).remove();
			}
		});
		form();
	}
	
	function form(){
		var Num = $('.main').find('.form-item-inner').first().find('option').length;//记录工作人员数量
		var itemNum = $('.main').find('.form-item-inner').length;//记录当前分派内容的数量，分派数量不得多于人数
		$('.main').find('.form-content').find('.glyphicon-plus').on('click', function () {
			if(itemNum < Num){
				var formItemInner = $('.main').find('.form-item-inner').last().clone();
				formItemInner.appendTo($('.main').find('.form-content').last());
				itemNum++;
				$('.main').find('.form-content').find('label').each(function(i){
					if(i>0){
						$(this).html(i+'、');
					}
				});
			}else{

			}

			$(".add-icon").css("display", "none");
			$(".add-icon:first").css("display", "block");
		});
	}

	function dataDeal(selector1,selector2,selector3,action){
		switch(action) {
			case 'get':
				$(selector1).find(selector2).find(selector3).each(function(i){
					$(this).val($(this).attr('value'));
				});
				break;
			case 'set':
				$(selector1).find(selector2).find(selector3).each(function(i){
					$(this).attr('value',$(this).val());
				});
				break;
			}
	}
});