(function () {
    //Section 1 : Code to execute when the toolbar button is pressed
    var a = {
        exec: function (editor) {
            //call display addImage Dialog
            showAddImageDialog();
        }
    },
    //Section 2 : Create the button and add the functionality to it
b = 'addImage';
    CKEDITOR.plugins.add(b, {
        init: function (editor) {
            editor.addCommand(b, a);
            editor.ui.addButton('addImage', {
                label: 'Add Image',
                icon: this.path + 'addImage.png',
                command: b
            });
        }
    });
})();