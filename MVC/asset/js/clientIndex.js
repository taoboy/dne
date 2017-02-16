$(function(){
	$('.sidebar li').click(function(){
		$(this).addClass('active').siblings('li').removeClass('active');
		$('.main .panel').eq($(this).index()).css('display','block').siblings('.panel').css('display','none');
	});
});