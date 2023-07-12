import Axios from "axios";

const state = () => ({
    loading: false
})

const mutations = {
    setLoading(state, value){
        state.loading = value
    }
}

const actions = {
    obtenerAmigos ({rootState}) {
        return new Promise((resolve, reject) => {
            Axios.get(`${rootState.api}api/friend/${rootState.usuario.nickname}`)
            .then(({data}) => {
                resolve(data)
            })
            .catch(error => {
                reject(error.response.data)
            })
        });
    },
    solicitarAmistad({rootState}, payload){
        return new Promise((resolve, reject) => {
            Axios.put(`${rootState.api}api/friend`, payload)
            .then(({data}) => {
                resolve(data)
            })
            .catch(error => {
                reject(error.response.data)
            })
        });
    },
    aceptarAmistad({rootState}, payload) {
        return new Promise((resolve, reject) => {
            Axios.post(`${rootState.api}api/friend`, payload)
            .then(({data}) => {
                resolve(data)
            })
            .catch(error => {
                reject(error.response.data.message)
            })
        });
    }
}

export default {
    namespaced: true,
    actions,
    mutations,
    state
}