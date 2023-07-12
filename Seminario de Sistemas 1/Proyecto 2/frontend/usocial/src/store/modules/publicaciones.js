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
    obtenerPublicaciones ({rootState}) {
        return new Promise((resolve, reject) => {
            Axios.get(`${rootState.api}api/publication/${rootState.usuario.nickname}`)
            .then(({data}) => {
                resolve(data)
            })
            .catch(error => {
                reject(error.response.data)
            })
        });
    },
    publicar({rootState}, payload){
        return new Promise((resolve, reject) => {
            Axios.put(`${rootState.api}api/publication`, payload)
            .then(({data}) => {
                resolve(data)
            })
            .catch(error => {
                reject(error.response.data)
            })
        });
    },
    traducir({rootState}, payload) {
        return new Promise((resolve, reject) => {
            Axios.post(`${rootState.api_translate}`, payload)
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