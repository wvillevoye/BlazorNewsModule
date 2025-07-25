window.quillBlazor = {
    editors: {},

    init: function (editorId, dotNetRef, imageUploadUrl, imageName, imageUrl) {
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
                        ['link', 'image', 'browse']
                        //[{ 'header': 3 }, { 'header': 4 }, { 'header': 5 }],      
                        //[{ 'script': 'sub' }, { 'script': 'super' }],       
                        //[{ 'size': ['small', false, 'large', 'huge'] }],   
                        //[{ 'header': [1, 2, 3, 4, 5, 6, false] }],         
                        //[{ 'color': [] }, { 'background': [] }],           
                        //[{ 'font': [] }],                                                              
                        //['link', 'image', 'video'],                        
                    ],
                    handlers: {
                        browse: function () {
                            window.quillBlazor.browse(editorId);
                        },
                        deleteImage: function () {
                            window.quillBlazor.deleteImage(imageName);
                        },
                      
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
        quill.__dotNetRef = dotNetRef;

        // Voeg <> knop toe aan de toolbar
        const toolbarElem = document.querySelector(`#${editorId}`).parentElement.querySelector('.ql-toolbar');
        if (toolbarElem) {
            const button = document.createElement('button');
            button.classList.add('ql-html');
            button.innerHTML = '&lt;&gt;';
            button.type = 'button';
            button.onclick = () => window.quillBlazor.toggleHtmlView(editorId);
            toolbarElem.appendChild(button);

            const browseButton = toolbarElem.querySelector('.ql-browse');
            if (browseButton) {
                browseButton.innerHTML = '<i class="bi bi-folder-fill"></i>';
            }
        }
    },

    browse: async function (editorId) {
        try {
            const response = await fetch('/api/quill/images');
            if (!response.ok) {
                console.error("Failed to fetch images");
                return;
            }

            const images = await response.json();
            const quill = window.quillBlazor.editors[editorId];

            if (quill && quill.__dotNetRef) {
                await quill.__dotNetRef.invokeMethodAsync('OnBrowseImages', images);
            } else {
                console.warn("DotNetRef not found for", editorId);
            }
        } catch (error) {
            console.error("Browse error:", error);
        }
    },

    insertImage: function (editorId, imageUrl) {
        const quill = this.editors[editorId];
        if (!quill) return;
        const range = quill.getSelection(true);
        quill.insertEmbed(range.index, 'image', imageUrl);
    },

    setContent: function (editorId, content) {
        const quill = this.editors[editorId];
        if (quill) {
            quill.root.innerHTML = content;
        }
    },

  

    deleteImage: async function (fileName) {
        if (!fileName) {
            // alert("Geen bestandsnaam opgegeven."); // Kan gedeactiveerd blijven
            return false; // Expliciet false retourneren
        }

        const confirmed = confirm(`Weet je zeker dat je "${fileName}" wilt verwijderen?`);
        if (!confirmed) {
            return false; // Expliciet false retourneren als de gebruiker annuleert
        }

        try {
            const response = await fetch(`/api/quill/images/${encodeURIComponent(fileName)}`, {
                method: "DELETE"
            });

            if (response.ok) {
                // alert("Afbeelding verwijderd."); // Kan gedeactiveerd blijven
                return true; // Expliciet true retourneren bij succes
            } else {
                const err = await response.text();
                // alert("Verwijderen mislukt: " + err); // Kan gedeactiveerd blijven
                console.error("Verwijderen mislukt:", err); // Log de fout voor debugging
                return false; // Expliciet false retourneren bij API-fout
            }
        } catch (e) {
            console.error("Fout bij verwijderen:", e);
            // alert("Er is een fout opgetreden."); // Kan gedeactiveerd blijven
            return false; // Expliciet false retourneren bij netwerkfout of andere uitzondering
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
window.blazorFocusModal = (modalId) => {
    setTimeout(() => {
        const modal = document.getElementById(modalId);
        if (modal) {
            const focusable = modal.querySelector('button, [href], input, select, textarea, [tabindex]:not([tabindex="-1"])');
            if (focusable) {
                focusable.focus();
            } else {
                modal.focus();
            }
        }
    }, 100); // 100 ms vertraging, kan ook 50 ms zijn
};
window.selectElementText = (elementId) => {
    const element = document.getElementById(elementId);
    if (element) {
        element.focus(); // Zorg dat het element focus heeft
        element.select(); // Selecteer alle tekst in het element
        element.setSelectionRange(0, 99999); // Voor mobiele browsers
    }
};
window.blazorFocusModal = (modalId) => {
    const modal = document.getElementById(modalId);
    if (modal) {
        modal.focus(); // Of focus een specifiek element in de modal
    }
};