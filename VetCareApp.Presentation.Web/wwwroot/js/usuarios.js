// Protección de acceso por rol
document.addEventListener('DOMContentLoaded', () => {
  const user = JSON.parse(localStorage.getItem('user'));
  if (!user) {
    window.location.href = 'index.html';
    return;
  }

  // Solo recepcionista puede entrar aquí
  if (user.rol !== 'Recepcionista') {
    alert('Acceso denegado. Solo el recepcionista puede acceder.');
    window.location.href = 'principal.html';
  }

  // Botón de logout
  const logoutBtn = document.getElementById('logoutBtn');
  logoutBtn.addEventListener('click', () => {
    localStorage.clear();
    window.location.href = 'index.html';
  });
});
