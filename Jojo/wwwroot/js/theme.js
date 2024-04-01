document.addEventListener('DOMContentLoaded', function () {
    const changeThemeBtn = document.getElementById('changeThemeBtn');

    changeThemeBtn.addEventListener('click', function () {
        const body = document.body;
        if (body.classList.contains('dark-mode')) {
            body.classList.remove('dark-mode');
            localStorage.setItem('theme', 'light');
        } else {
            body.classList.add('dark-mode');
            localStorage.setItem('theme', 'dark');
        }
    });

    const theme = localStorage.getItem('theme');
    if (theme && theme === 'dark') {
        document.body.classList.add('dark-mode');
    }
});
