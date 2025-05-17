function showSimpleAlert(icon, title){
    Swal.fire({
        toast: true,
        position: "top-end",
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
        confirmButtonText: `Già esistente <i class="fa-solid fa-user ms-2"></i>`,
        cancelButtonColor: "#2c3e50",
        cancelButtonText: `Crea nuovo <i class="fa-solid fa-plus ms-2"></i>`,
        allowOutsideClick: true,
        allowEscapeKey: true
    }).then((result) => {
        if (result.isConfirmed) {
            openSearchUserAlert();
        } else if(result.dismiss === Swal.DismissReason.cancel){
            let eventModal = new bootstrap.Modal(document.getElementById('eventModal'));
            eventModal.show();
        }
    });
}

function showEventAlert(event){
    let startTime = new Date(event.event.start).toLocaleTimeString('it-IT', { hour: '2-digit', minute: '2-digit', hour12: false });
    let endTime = new Date(event.event.end).toLocaleTimeString('it-IT', { hour: '2-digit', minute: '2-digit', hour12: false });
    let dateFormatted = new Date(event.event.start).toLocaleDateString('it-IT');

    Swal.fire({
        title: event.event.title,
        icon: "info",
        html: `
            <div class="container ps-5 d-flex flex-column align-items-start">
                <p><b>Cliente: </b>${event.customer.name} ${event.customer.surname}</p>
                <p><b>Telefono: </b>${event.customer.phone}</p>
                <p><b>Email: </b>${event.customer.email}</p>
                <p><b>Data: </b>${dateFormatted} ${startTime} - ${endTime}</p>
                <p><b>Note: </b>${event.event.notes}</p>
            </div>
        `,
        showConfirmButton: true,
        confirmButtonText: `Modifica <i class="fa-solid fa-pen ms-2"></i>`,
        confirmButtonColor: "#0d6efd",
        showCloseButton: true,
        showCancelButton: true,
        focusConfirm: false,
        cancelButtonText: "Chiudi",
    }).then((result) => {
        if (result.isConfirmed) {
            window.location.href = `/Home/EditEvent/${event.event.id}`;
        }
    });
}


async function showAlertDeleteEvent(eventId){
    Swal.fire({
        title: "Sei sicuro?",
        text: "Eliminare questo evento non è reversibile!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#d33",
        confirmButtonText: "Elimina",
        cancelButtonColor: "#6c757d",
        cancelButtonText: "Annulla"
    }).then(async (result) => {
        try {
            const response = await fetch(`/Home/DeleteEvent?eventId=${encodeURIComponent(eventId)}`, {
                method: "POST",
            });
            if (!response.ok) showSimpleAlert("error", "Si è verificato un errore nella richiesta.");
            window.location.href = "/Home/Index";
            showSimpleAlert("success", "Evento eliminato con successo!");
        } catch (err) {
            showSimpleAlert("error", "Si è verificato un errore durante l'eliminazione.");
        }
    });
}