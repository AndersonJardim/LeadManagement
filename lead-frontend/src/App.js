import React, { useState, useEffect } from 'react';

const API_BASE_URL = 'https://localhost:7080/api/leads';

function App() {
  const [leads, setLeads] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [activeTab, setActiveTab] = useState('invited');

  const [formData, setFormData] = useState({
    contactFirstName: '',
    suburb: '',
    category: '',
    description: '',
    price: '',
  });
  const [editingLeadId, setEditingLeadId] = useState(null);

  // Dados fixos para a aba 'Invited'
  const invitedLeads = [
    {
      id: 'invited-1',
      contactFirstName: 'Bill',
      dateCreated: '2023-01-04T14:37:00Z',
      suburb: 'Yanderra 2574',
      category: 'Painters',
      description: 'Need to paint 2 aluminum windows and a sliding glass door',
      price: 62.00,
    },
    {
      id: 'invited-2',
      contactFirstName: 'Craig',
      dateCreated: '2023-01-04T13:15:00Z',
      suburb: 'Woolooware 2230',
      category: 'Interior Painters',
      description: 'internal walls 3 colours',
      price: 49.00,
    },
  ];

  // Dados fixos para a aba 'Accepted', baseados no seu template
  const acceptedLeads = [
    {
      id: 'accepted-1',
      contactFirstName: 'Pete',
      dateCreated: '2018-09-05T10:36:00Z',
      suburb: 'Carramar 6031',
      category: 'General Building Work',
      description: 'Plaster exposed brick walls (see photos), square off 2 archways (see photos), and expand pantry (see photos).',
      price: 26.00,
      contactPhoneNumber: '0412345678',
      contactEmail: 'fake@mailinator.com',
    },
    {
      id: 'accepted-2',
      contactFirstName: 'Chris',
      dateCreated: '2018-08-30T11:14:00Z',
      suburb: 'Quinns Rocks 6030',
      category: 'Home Renovations',
      description: 'There is a two story building at the front of the main house that\'s about 10x5 that would like to convert into self contained living area.',
      price: 32.00,
      contactPhoneNumber: '04987654321',
      contactEmail: 'another.fake@hipmail.com',
    },
  ];

  useEffect(() => {
    fetchLeads();
  }, []);

  const fetchLeads = async () => {
    setLoading(true);
    setError(null);
    try {
      const response = await fetch(API_BASE_URL);
      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
      }
      const data = await response.json();
      setLeads(data);
    } catch (err) {
      console.error("Erro ao buscar leads:", err);
      setError("Erro ao carregar os leads. Por favor, tente novamente.");
    } finally {
      setLoading(false);
    }
  };

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    setFormData({
      ...formData,
      [name]: name === 'price' ? parseFloat(value) || '' : value,
    });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);
    setError(null);

    const method = editingLeadId ? 'PUT' : 'POST';
    const url = editingLeadId ? `${API_BASE_URL}/${editingLeadId}` : API_BASE_URL;

    try {
      const response = await fetch(url, {
        method: method,
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(formData),
      });

      if (!response.ok) {
        const errorText = await response.text();
        throw new Error(`HTTP error! status: ${response.status}, Details: ${errorText}`);
      }

      setFormData({
        contactFirstName: '',
        suburb: '',
        category: '',
        description: '',
        price: '',
      });
      setEditingLeadId(null);
      fetchLeads();
    } catch (err) {
      console.error("Erro ao salvar lead:", err);
      setError(`Erro ao salvar o lead: ${err.message}`);
    } finally {
      setLoading(false);
    }
  };

  const handleEdit = (lead) => {
    setEditingLeadId(lead.id);
    setFormData({
      contactFirstName: lead.contactFirstName,
      suburb: lead.suburb,
      category: lead.category,
      description: lead.description,
      price: lead.price,
    });
    window.scrollTo({ top: 0, behavior: 'smooth' });
  };

  const handleDelete = async (id) => {
    if (!window.confirm("Tem certeza que deseja deletar este lead?")) {
      return;
    }

    setLoading(true);
    setError(null);
    try {
      const response = await fetch(`${API_BASE_URL}/${id}`, {
        method: 'DELETE',
      });

      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
      }

      fetchLeads();
    } catch (err) {
      console.error("Erro ao deletar lead:", err);
      setError("Erro ao deletar o lead. Por favor, tente novamente.");
    } finally {
      setLoading(false);
    }
  };

  const LeadCard = ({ lead, isAccepted = false, onEdit, onDelete }) => {
    const avatarLetter = lead.contactFirstName ? lead.contactFirstName.charAt(0).toUpperCase() : '?';
    const avatarColor = lead.id.endsWith('1') ? '#ff9800' : lead.id.endsWith('2') ? '#ff7043' : '#a0a0a0';

    return (
      <div className="bg-white border border-gray-200 rounded-lg mb-5 p-5 shadow-sm">
        <div className="flex items-center mb-4">
          <div className="w-10 h-10 rounded-full flex items-center justify-center text-lg font-medium mr-4 flex-shrink-0 text-white" style={{ backgroundColor: avatarColor }}>
            {avatarLetter}
          </div>
          <div className="user-info">
            <h3 className="m-0 text-lg font-semibold text-gray-800">{lead.contactFirstName}</h3>
            <p className="m-0 text-sm text-gray-600">{new Date(lead.dateCreated).toLocaleString('pt-BR', { month: 'long', day: 'numeric', year: 'numeric', hour: 'numeric', minute: 'numeric' })}</p>
          </div>
        </div>
        <div className="flex flex-wrap items-center mb-4 text-gray-600 text-base">
          <span className="flex items-center mr-5 mb-1">
            <svg className="w-4 h-4 fill-current text-gray-400 mr-2 flex-shrink-0" viewBox="0 0 24 24">
              <path d="M12 2c-4.419 0-8 3.582-8 8a8.078 8.078 0 0 0 2.828 5.922L12 22l5.172-6.078A8.078 8.078 0 0 0 20 10c0-4.418-3.581-8-8-8zm0 11.5c-1.933 0-3.5-1.567-3.5-3.5s1.567-3.5 3.5-3.5 3.5 1.567 3.5 3.5-1.567 3.5-3.5 3.5z" />
            </svg>
            {lead.suburb}
          </span>
          <span className="flex items-center mr-5 mb-1">
            <svg className="w-4 h-4 fill-current text-gray-400 mr-2 flex-shrink-0" viewBox="0 0 24 24">
              <path d="M20 6h-4V4c0-1.103-.897-2-2-2h-4c-1.103 0-2 .897-2 2v2H4c-1.103 0-2 .897-2 2v11c0 1.103.897 2 2 2h16c1.103 0 2-.897 2-2V8c0-1.103-.897-2-2-2zm-6-2h-4v2h4V4zm6 15H4V8h16v11z" />
            </svg>
            {lead.category}
          </span>
          <span className="mb-1">Job ID: {lead.id.substring(0, 8)}</span>
        </div>

        {isAccepted && (
          <div className="flex flex-wrap mb-4 text-gray-600 text-base">
            {lead.contactPhoneNumber && (
              <span className="flex items-center mr-5 mb-1">
                <svg className="w-4 h-4 fill-current text-gray-400 mr-2 flex-shrink-0" viewBox="0 0 24 24">
                  <path d="M6.62 10.79a15.018 15.018 0 0 0 6.59 6.59l2.21-2.21a.996.996 0 0 1 1.05-.22c1.37.45 2.81.69 4.29.69.55 0 1 .45 1 1V20c0 .55-.45 1-1 1-1.27 0-2.5-.23-3.65-.68-1.56-.6-3.08-1.59-4.4-2.91-1.32-1.32-2.31-2.84-2.91-4.4-1.15-2.47-1.15-5.32 0-7.79.45-1.17.69-2.39.69-3.65 0-.55-.45-1-1-1H4c-.55 0-1 .45-1 1 0 1.48.24 2.92.69 4.29z" />
                </svg>
                {lead.contactPhoneNumber}
              </span>
            )}
            {lead.contactEmail && (
              <span className="flex items-center mb-1">
                <svg className="w-4 h-4 fill-current text-gray-400 mr-2 flex-shrink-0" viewBox="0 0 24 24">
                  <path d="M20 4H4c-1.1 0-1.99.9-1.99 2L2 18c0 1.1.9 2 2 2h16c1.1 0 2-.9 2-2V6c0-1.1-.9-2-2-2zm0 4l-8 5-8-5V6l8 5 8-5v2z" />
                </svg>
                {lead.contactEmail}
              </span>
            )}
          </div>
        )}

        <p className="job-description text-base text-gray-700 leading-relaxed mb-5">
          {lead.description}
        </p>

        <div className="flex flex-col md:flex-row items-start md:items-center justify-between gap-4 md:gap-0">
          {!isAccepted && (
            <div className="flex w-full md:w-auto gap-2">
              <button
                className="flex-grow md:flex-grow-0 px-4 py-2 rounded-md text-base cursor-pointer transition-colors duration-300 text-white bg-orange-500 border border-orange-500 hover:bg-orange-600 hover:border-orange-600"
                onClick={() => alert(`Accept Lead ${lead.contactFirstName}`)}
              >
                Accept
              </button>
              <button
                className="flex-grow md:flex-grow-0 px-4 py-2 rounded-md text-base cursor-pointer transition-colors duration-300 bg-gray-100 text-gray-600 border border-gray-300 hover:bg-gray-200 hover:border-gray-400"
                onClick={() => alert(`Decline Lead ${lead.contactFirstName}`)}
              >
                Decline
              </button>
            </div>
          )}
          <div className="text-lg font-medium text-gray-800 w-full md:w-auto text-left md:text-right">
            $ {lead.price ? lead.price.toFixed(2) : '0.00'} Lead Invitation
          </div>
        </div>
      </div>
    );
  };

  return (
    <div className="font-roboto flex justify-center p-5 min-h-screen box-border bg-[#f0f2f5]">
      <div className="bg-white rounded-lg shadow-xl w-full max-w-lg md:max-w-xl lg:max-w-2xl xl:max-w-3xl overflow-hidden box-border">
        {/* Formulário de Criação/Edição */}
        <div className="p-6">
          <h2 className="text-2xl font-semibold mb-6 text-gray-800">{editingLeadId ? 'Editar Lead' : 'Criar Novo Lead'}</h2>
          <form onSubmit={handleSubmit} className="grid grid-cols-1 md:grid-cols-2 gap-6">
            <div>
              <label htmlFor="contactFirstName" className="block text-sm font-medium text-gray-700 mb-1">Primeiro Nome do Contato</label>
              <input
                type="text"
                id="contactFirstName"
                name="contactFirstName"
                value={formData.contactFirstName}
                onChange={handleInputChange}
                required
                className="mt-1 block w-full px-4 py-2 border border-gray-300 rounded-md shadow-sm focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"
              />
            </div>
            <div>
              <label htmlFor="suburb" className="block text-sm font-medium text-gray-700 mb-1">Bairro</label>
              <input
                type="text"
                id="suburb"
                name="suburb"
                value={formData.suburb}
                onChange={handleInputChange}
                required
                className="mt-1 block w-full px-4 py-2 border border-gray-300 rounded-md shadow-sm focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"
              />
            </div>
            <div>
              <label htmlFor="category" className="block text-sm font-medium text-gray-700 mb-1">Categoria</label>
              <input
                type="text"
                id="category"
                name="category"
                value={formData.category}
                onChange={handleInputChange}
                required
                className="mt-1 block w-full px-4 py-2 border border-gray-300 rounded-md shadow-sm focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"
              />
            </div>
            <div>
              <label htmlFor="price" className="block text-sm font-medium text-gray-700 mb-1">Preço (R$)</label>
              <input
                type="number"
                id="price"
                name="price"
                value={formData.price}
                onChange={handleInputChange}
                required
                step="0.01"
                className="mt-1 block w-full px-4 py-2 border border-gray-300 rounded-md shadow-sm focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"
              />
            </div>
            <div className="col-span-1 md:col-span-2">
              <label htmlFor="description" className="block text-sm font-medium text-gray-700 mb-1">Descrição</label>
              <textarea
                id="description"
                name="description"
                value={formData.description}
                onChange={handleInputChange}
                rows="3"
                className="mt-1 block w-full px-4 py-2 border border-gray-300 rounded-md shadow-sm focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"
              ></textarea>
            </div>
            <div className="col-span-1 md:col-span-2 flex justify-end space-x-3">
              <button
                type="submit"
                className="inline-flex justify-center py-2 px-6 border border-transparent shadow-sm text-sm font-medium rounded-md text-white bg-indigo-600 hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500 transition duration-150 ease-in-out"
                disabled={loading}
              >
                {loading ? 'Salvando...' : (editingLeadId ? 'Atualizar Lead' : 'Criar Lead')}
              </button>
              {editingLeadId && (
                <button
                  type="button"
                  onClick={() => {
                    setEditingLeadId(null);
                    setFormData({
                      contactFirstName: '',
                      suburb: '',
                      category: '',
                      description: '',
                      price: '',
                    });
                  }}
                  className="inline-flex justify-center py-2 px-6 border border-gray-300 shadow-sm text-sm font-medium rounded-md text-gray-700 bg-white hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500 transition duration-150 ease-in-out"
                >
                  Cancelar Edição
                </button>
              )}
            </div>
          </form>
        </div>

        {/* Mensagens de estado */}
        {loading && activeTab === 'invited' && (
          <p className="text-center text-lg text-indigo-600 font-medium mb-4">Carregando leads...</p>
        )}
        {error && (
          <div className="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded-md relative mb-4 mx-6" role="alert">
            <strong className="font-bold">Erro!</strong>
            <span className="block sm:inline ml-2">{error}</span>
          </div>
        )}

        <div className="flex border-b border-gray-200">
          <div
            className={`flex-1 py-4 text-center font-medium text-gray-600 cursor-pointer relative transition-colors duration-300 ${activeTab === 'invited' ? 'tab-active' : ''}`}
            onClick={() => setActiveTab('invited')}
          >
            Invited
          </div>
          <div
            className={`flex-1 py-4 text-center font-medium text-gray-600 cursor-pointer relative transition-colors duration-300 ${activeTab === 'accepted' ? 'tab-active' : ''}`}
            onClick={() => setActiveTab('accepted')}
          >
            Accepted
          </div>
        </div>

        <div className="p-5">
          {activeTab === 'invited' && (
            <div>
              {leads.length === 0 && !loading && !error ? (
                <p className="text-center text-gray-500">Nenhum lead convidado encontrado. Crie um novo!</p>
              ) : (
                leads.map((lead) => (
                  <LeadCard
                    key={lead.id}
                    lead={lead}
                    onEdit={handleEdit}
                    onDelete={handleDelete}
                  />
                ))
              )}
            </div>
          )}

          {activeTab === 'accepted' && (
            <div>
              {acceptedLeads.map((lead) => (
                <LeadCard
                  key={lead.id}
                  lead={lead}
                  isAccepted={true}
                />
              ))}
            </div>
          )}
        </div>
      </div>
    </div>
  );
}

export default App;
