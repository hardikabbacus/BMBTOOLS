CKEDITOR.editorConfig = function(config) {
    config.language = 'en';
    config.uiColor='#E0D9C6';
    //config.toolbar = [['Source'], ['Bold', 'Italic'], ['NumberedList', 'BulletedList'], ['-', 'Link']];
    config.enterMode = CKEDITOR.ENTER_BR;
    config.toolbar = [['Source'], ['Bold', 'Italic', 'Underline'], ['NumberedList', 'BulletedList'], ['-', 'Link'], ['Format'], ['Font'], ['FontSize']];
};
