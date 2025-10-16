const form = document.getElementById('loginForm');
const errorMsg = document.getElementById('error-msg');

form.addEventListener('submit', async (e) => {
  e.preventDefault();

  const data = {
    username: document.getElementById('username').value,
    password: document.getElementById('password').value
  };

  try {
    // Cambia esta URL a la de tu backend local
    const response = await fetch('https://localhost:7071/api/Usuarios/login', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(data)
    });

    if (!response.ok) throw new Error('Error en el inicio de sesión');

    const user = await response.json();

    // Guardamos el usuario en localStorage
    localStorage.setItem('user', JSON.stringify(user));

    // Redirección según rol
    if (user.rol === 'Recepcionista') {
      window.location.href = 'dashboard.html';
    } else {
      window.location.href = 'principal.html';
    } 

  } catch (err) {
    errorMsg.textContent = 'Credenciales inválidas o error en el servidor.';
  }
});
