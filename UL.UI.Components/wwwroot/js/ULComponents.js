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