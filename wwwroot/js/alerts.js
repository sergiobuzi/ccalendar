function showSimpleAlert(icon, title){
    Swal.fire({
        icon: icon,
        title: title,
        showConfirmButton: false,
        timer: 2000
    });
}

// function showOptionsAlert(){
//     Swal.fire({
//         title: "Do you want to save the changes?",
//         showDenyButton: true,
//         showCancelButton: true,
//         confirmButtonText: "Save",
//         denyButtonText: `Don't save`
//         }).then((result) => {
//         /* Read more about isConfirmed, isDenied below */
//         if (result.isConfirmed) {
//             Swal.fire("Saved!", "", "success");
//         } else if (result.isDenied) {
//             Swal.fire("Changes are not saved", "", "info");
//         }
//     });
// }
