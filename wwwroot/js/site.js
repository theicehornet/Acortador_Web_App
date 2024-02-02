
document.querySelector("#btn_subac").addEventListener("click", handleClick);

function handleClick(event) {
    event.preventDefault();
    let link = document.getElementById("link").value;
    let message = document.getElementById("message");
    message.innerHTML = "";
    let prevLink = localStorage.getItem('prevLink');
    if (!(link.startsWith('http') || link.startsWith('https'))) {
        message.innerHTML = "Debe ser un link, no puede ser un texto";
        return;
    }
    if (link === prevLink) {
        // No es necesario enviar el enlace al servidor
        console.log("El enlace es el mismo que el anterior, no se enviará al servidor.");
        return;
    }
    localStorage.setItem('prevLink', link);
    document.querySelector("#form_acortador").submit();
}