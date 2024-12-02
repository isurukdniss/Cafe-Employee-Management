import axios from 'axios';

const API_BASE_URL = "https://localhost:7199/api";

// Cafe APIs
export const getCafes = () => axios.get(`${API_BASE_URL}/cafe`);
export const getCafe = (id) => axios.get(`${API_BASE_URL}/cafe/${id}`);
export const createCafe = (formData) => {
    return axios.post(`${API_BASE_URL}/cafe`, formData, {
        headers: {
            'Content-Type': 'multipart/form-data',
        },
    });
}

export const updateCafe = (id, formData) => {
    return axios({
        url: `${API_BASE_URL}/cafe/${id}`,
        method: "PUT",
        data: formData,
    })
}
export const deleteCafe = (id) => axios.delete(`${API_BASE_URL}/cafe/${id}`);

// Employee APIs
export const getEmployees = () => axios.get(`${API_BASE_URL}/employee`);
export const getEmployee = (id) => axios.get(`${API_BASE_URL}/employee/${id}`);
export const createEmployee = (data) => axios.post(`${API_BASE_URL}/employee`, data);
export const updateEmployee = (id, data) => axios.put(`${API_BASE_URL}/employee/${id}`, data);
export const deleteEmployee = (id) => axios.delete(`${API_BASE_URL}/employee/${id}`);


