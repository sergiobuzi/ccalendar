window.addEventListener('load', function () {
    setTimeout(function () {
        const loader = document.getElementById('loader');
        if (loader) {
            loader.style.transition = 'opacity 0.6s ease';
            loader.style.opacity = '0';

            setTimeout(function () {
                loader.remove();
            }, 600);
        }
    }, 1000);
});