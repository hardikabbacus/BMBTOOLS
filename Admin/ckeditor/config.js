﻿/*
Copyright (c) 2003-2009, CKSource - Frederico Knabben. All rights reserved.
For licensing, see LICENSE.html or http://ckeditor.com/license
*/

CKEDITOR.editorConfig = function(config) {
    // Define changes to default configuration here. For example:
    // config.language = 'fr';
    // config.uiColor = '#AADC6E';
config.enterMode = CKEDITOR.ENTER_BR;
config.toolbar = [['Source'], ['Bold', 'Italic', 'Underline'], ['NumberedList', 'BulletedList'], ['-', 'Link'], ['Format'], ['Font'], ['FontSize']];
};
