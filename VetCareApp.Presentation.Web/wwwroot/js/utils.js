// ============================================
// 2. utils.js - Funciones compartidas
// ============================================
const Utils = {
  // Selector rápido
  q: (id) => document.getElementById(id),

  // Event listener helper
  on: (el, evt, cb) => {
    if (el) el.addEventListener(evt, cb);
  },

  // Parse JSON seguro
  safeJSON: (res) => {
    return res.text().then(text => {
      try {
        return text ? JSON.parse(text) : null;
      } catch {
        return null;
      }
    });
  },

  // Badge de estado para citas
  getEstadoBadge: (estado) => {
    const estados = {
      'Pendiente': '<span style="background: #FEF3C7; color: #92400E; padding: 0.25rem 0.6rem; border-radius: 12px;">Pendiente</span>',
      'Confirmada': '<span style="background: #D1FAE5; color: #065F46; padding: 0.25rem 0.6rem; border-radius: 12px;">Confirmada</span>',
      'Completada': '<span style="background: #DBEAFE; color: #1E40AF; padding: 0.25rem 0.6rem; border-radius: 12px;">Completada</span>',
      'Cancelada': '<span style="background: #FEE2E2; color: #991B1B; padding: 0.25rem 0.6rem; border-radius: 12px;">Cancelada</span>'
    };
    return estados[estado] || `<span>${estado}</span>`;
  },

  // Modal helpers
  abrirModal: (title, innerHtml, onSubmit) => {
    const modal = Utils.q('modal');
    const modalTitle = Utils.q('modalTitle');
    const modalFormContent = Utils.q('modalFormContent');
    
    modalTitle.textContent = title;
    modalFormContent.innerHTML = innerHtml;
    modal.classList.add('active');

    const form = Utils.q('modalForm');
    form.onsubmit = async (e) => {
      e.preventDefault();
      await onSubmit();
    };
  },

  cerrarModal: () => {
    const modal = Utils.q('modal');
    if (modal) modal.classList.remove('active');
  },

  // Fetch helper con autenticación
  fetchAPI: async (endpoint, options = {}) => {
    const token = CONFIG.getToken();
    const defaultHeaders = {
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    };

    return fetch(`${CONFIG.API_URL}${endpoint}`, {
      ...options,
      headers: { ...defaultHeaders, ...options.headers }
    });
  }
};

window.Utils = Utils;