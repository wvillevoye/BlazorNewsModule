window.quillBlazor = {
    editors: {},

    init: function (editorId, dotNetRef, imageUploadUrl) {
        const quill = new Quill(`#${editorId}`, {
            theme: 'snow',
            modules: {
                toolbar: {
                    container: [
                        ['bold', 'italic', 'underline', 'strike'],         
                        ['blockquote', 'code-block'],
                        [{ 'header': [3, 4, 5, 6, false] }],                   
                        [{ 'list': 'ordered' }, { 'list': 'bullet' }],     
                        [{ 'align': [] }], 
                        [{ 'indent': '-1' }, { 'indent': '+1' }],           
                        [{ 'direction': 'rtl' }],
                        ['clean'],
                        ['link', 'image']
                        //[{ 'header': 3 }, { 'header': 4 }, { 'header': 5 }],      
                        //[{ 'script': 'sub' }, { 'script': 'super' }],       
                        //[{ 'size': ['small', false, 'large', 'huge'] }],   
                        //[{ 'header': [1, 2, 3, 4, 5, 6, false] }],         
                        //[{ 'color': [] }, { 'background': [] }],           
                        //[{ 'font': [] }],                                                              
                        //['link', 'image', 'video'],                        
                    ],
                    handlers: {
                        image: function () {
                            const input = document.createElement('input');
                            input.setAttribute('type', 'file');
                            input.setAttribute('accept', 'image/*');
                            input.click();

                            input.onchange = async () => {
                                const file = input.files[0];
                                if (file) {
                                    if (file.size > 1 * 1024 * 1024) {  // 3MB limiet
                                        alert("Bestand is te groot. Maximaal 1MB toegestaan.");
                                        return;  // Stop uploaden!
                                    }


                                    const formData = new FormData();
                                    formData.append("image", file);

                                    const response = await fetch(imageUploadUrl, {
                                        method: "POST",
                                        body: formData
                                    });

                                    if (!response.ok) {
                                        const text = await response.text();
                                        console.error("Upload failed:", text);
                                        return;
                                    }

                                    const contentType = response.headers.get("Content-Type") || "";
                                    if (!contentType.includes("application/json")) {
                                        alert("Server returned invalid content type.");
                                        return;
                                    }

                                    const result = await response.json();
                                    const range = quill.getSelection(true);
                                    quill.insertEmbed(range.index, 'image', result.url);
                                }
                            };
                        }
                        
                    }
                }
            }
        });

        quill.on('text-change', function () {
            dotNetRef.invokeMethodAsync('OnContentChanged', quill.root.innerHTML);
        });

        this.editors[editorId] = quill;

        // Voeg <> knop toe aan de toolbar
        const toolbarElem = document.querySelector(`#${editorId}`).parentElement.querySelector('.ql-toolbar');
        if (toolbarElem) {
            const button = document.createElement('button');
            button.classList.add('ql-html');
            button.innerHTML = '&lt;&gt;';
            button.type = 'button';
            button.onclick = () => window.quillBlazor.toggleHtmlView(editorId);
            toolbarElem.appendChild(button);
        }
    },

    setContent: function (editorId, content) {
        const quill = this.editors[editorId];
        if (quill) {
            quill.root.innerHTML = content;
        }
    },

    destroy: function (editorId) {
        delete this.editors[editorId];
    },

    toggleHtmlView: function (editorId) {
        const quill = this.editors[editorId];
        if (!quill) return;

        const container = quill.root.parentNode;
        let textarea = container.querySelector('textarea[data-htmlview]');

        if (textarea) {
            // Terug naar WYSIWYG
            quill.root.innerHTML = textarea.value;
            textarea.remove();
            quill.root.style.display = '';
        } else {
            // Naar HTML-bronweergave
            textarea = document.createElement('textarea');
            textarea.setAttribute('data-htmlview', 'true');
            textarea.className = 'form-control mt-2';
            textarea.style.height = '400px';
            textarea.value = quill.root.innerHTML;

            quill.root.style.display = 'none';
            container.appendChild(textarea);
        }
    }
};
