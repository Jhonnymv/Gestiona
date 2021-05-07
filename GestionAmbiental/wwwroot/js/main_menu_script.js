const colors = ['#CD3728', '#00BCE4', '#E6298D', '#C1D82F'];
$('.menu_item').mouseenter(function (){
    const rand = Math.floor(Math.random() * 4);

    $(this).find('svg path').attr('style', 'fill: '+colors[rand]+'');
    $(this).find('span').attr('style', 'color: '+colors[rand]+'');

    $(this).mouseleave(function (){
        $(this).find('svg path').attr('style', 'fill: #2D649B');
        $(this).find('span').attr('style', 'color: #2D649B');
    });
});