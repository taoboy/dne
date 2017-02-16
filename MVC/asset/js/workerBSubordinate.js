$(function(){
	$('.sidebar li').click(function(){
		$(this).addClass('active').siblings('li').removeClass('active');
		$('.main .panel').eq($(this).index()).css('display','block').siblings('.panel').css('display','none');
	});
	var peopleNum = $('.main').find('.distribute-task').find('select option').length;
	var itemNum = $('.main').find('.distribute-task').find('select').length;
	var submit = $('.main').find('.distribute-task').find('.form-group').last();
	var html = 	'<div class="form-group"><div class="col-sm-1"></div>';
		html += '<label class="col-sm-2 control-label">1、</label>';
		html += '<div class="col-sm-6"><select class="form-control">';
		html += '<option>甲</option><option>乙</option><option>丙</option>';
		html += '<option>丁</option></select></div><div class="col-sm-1">';
		html += '<span class="glyphicon glyphicon-minus"></span></div></div>';
		html += '<div class="form-group"><div class="col-sm-3"></div>';
		html += '<div class="col-sm-6"><textarea class="form-control" rows="3">';
		html += '</textarea></div></div>';
	$('.main').find('.distribute-task').find('.glyphicon-plus').click(function(){
		if(itemNum < peopleNum){
			submit.before(html);
			itemNum++;
			$('.main').find('.distribute-task').find('.glyphicon-minus').off().on('click',function(){
				$(this).parents('.form-group').next('.form-group').remove();
				$(this).parents('.form-group').remove();
				itemNum--;
				$('.main').find('.distribute-task').find('label').each(function(i){
					if(i > 1){
						$(this).text(i-1+'、');
					}
				});
			});
			$('.main').find('.distribute-task').find('label').each(function(i){
				if(i > 1){
					$(this).text(i-1+'、');
				}
			});
		}
	});
	$('.main .check-task').find('.pass-task').on('click',function(){
		$('.main .check-task').find('.alert-success').fadeIn('400', function(){
			setTimeout(function(){
				$('.main .check-task').find('.alert-success').fadeOut('400');
			},1000);
		});
	});
	$('.main .distribute-task').find('.pass-task').on('click',function(){
		$('.main .distribute-task').find('.alert-success').fadeIn('400', function(){
			setTimeout(function(){
				$('.main .distribute-task').find('.alert-success').fadeOut('400');
			},1000);
		});
	});
});