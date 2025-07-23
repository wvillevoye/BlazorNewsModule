window.tinymceBlazor = {
    init: function (id, dotNetRef, toolbar, plugins) {
        tinymce.init({
            selector: '#' + id,
            plugins: plugins || 'advlist autolink lists link image charmap print preview anchor searchreplace visualblocks code fullscreen insertdatetime media table paste code help wordcount',
            toolbar: toolbar || 'undo redo | formatselect | bold italic backcolor | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | removeformat | help',
            height: 400,
            menubar: false,
            statusbar: false,
            setup: function (editor) {
                editor.on('blur', function () {
                    // Roep de .NET methode aan wanneer de editor de focus verliest
                    dotNetRef.invokeMethodAsync('SetContent', editor.getContent());
                });
                editor.on('change', function () {
                    // Roep de .NET methode aan wanneer de inhoud verandert (optioneel, kan veel calls veroorzaken)
                    dotNetRef.invokeMethodAsync('SetContent', editor.getContent());
                });
            }
        });
    },
    getContent: function (id) {
        if (tinymce.get(id)) {
            return tinymce.get(id).getContent();
        }
        return '';
    },
    setContent: function (id, content) {
        if (tinymce.get(id)) {
            tinymce.get(id).setContent(content);
        }
    },
    destroy: function (id) {
        if (tinymce.get(id)) {
            tinymce.get(id).destroy();
        }
    }
};