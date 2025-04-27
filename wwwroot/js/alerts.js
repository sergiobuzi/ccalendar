function showSimpleAlert(icon, title){
    Swal.fire({
        icon: icon,
        title: title,
        showConfirmButton: false,
        timer: 2000
    });
}

function getInfoAlert() {
    Swal.fire({
        text: "Creane uno nuovo cliente oppure selezionane uno già esistente",
        icon: "question",
        showCancelButton: true,
        confirmButtonColor: "#0d6efd",
        confirmButtonText: "Già esistente",
        cancelButtonColor: "#2c3e50",
        cancelButtonText: "Crea nuovo"
    }).then((result) => {
        if (result.isConfirmed) {
            openSearchUserAlert();
        } else {
            let eventModal = new bootstrap.Modal(document.getElementById('eventModal'));
            eventModal.show();
        }
    });
}