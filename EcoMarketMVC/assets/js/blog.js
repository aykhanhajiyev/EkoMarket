$(document).ready(function()
{"use strict";var menu=$('.menu');var burger=$('.hamburger');var menuActive=false;$(window).on('resize',function()
{setTimeout(function()
{$(window).trigger('resize.px.parallax');},375);});initMenu();function initMenu()
{if(menu.length)
{if($('.hamburger').length)
{burger.on('click',function()
{if(menuActive)
{closeMenu();}
else
{openMenu();$(document).one('click',function cls(e)
{if($(e.target).hasClass('menu_mm'))
{$(document).one('click',cls);}
else
{closeMenu();}});}});}}}
function openMenu()
{menu.addClass('active');menuActive=true;}
function closeMenu()
{menu.removeClass('active');menuActive=false;}});