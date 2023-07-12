import Axios from "axios";

const state = () => ({
    nickname: '',
    loading: false
})

const mutations = {
    setNickname(state, value){
        state.nickname = value
    },
    setLoading(state, value){
        state.loading = value
    }
}

const actions = {
    covid({rootState}, value){
        return new Promise((resolve, reject) => {
            Axios.get(`${rootState.api_covid}&where=Country_Region%20%3D%20'${value.toUpperCase()}'`)
            .then(({data}) => {
                let values = [
                    `Los casos confirmados son ${data.features[0].attributes.Confirmed} con una tasa de infeccion de ${Math.round(data.features[0].attributes.Incident_Rate * 100) / 100} infectados por millon`,
                    `${data.features[0].attributes.Deaths} son las muertes confimadas con una tasa de mortalidad de ${Math.round(data.features[0].attributes.Mortality_Rate * 100) / 100}%`
                ]
                resolve(values)
            })
            .catch(err => {
                reject(err)
            })
        });
    },
    editarUser ({rootState}, payload) {
        return new Promise((resolve, reject) => {
            Axios.post(`${rootState.api}api/user`, payload)
            .then(({data}) => {
                resolve(data)
            })
            .catch(error => {
                reject(error.response.data)
            })
        });
    },
    userData({state, rootState}){
        return new Promise((resolve, reject) => {
            Axios.get(`${rootState.api}api/user/${state.nickname}`)
            .then(({data}) => {
                resolve(data)
            })
            .catch(error => {
                reject(error.response.data)
            })
        });
    },
    registrar({rootState}, payload) {
        return new Promise((resolve, reject) => {
            Axios.put(`${rootState.api}api/user`, payload)
            .then(({data}) => {
                resolve(data.message)
            })
            .catch(error => {
                reject(error.response.data.message)
            })
        });
    },
    password({rootState, commit}, payload) {
        return new Promise((resolve, reject) => {
            Axios.post(`${rootState.api}api/authentication/`, payload)
            .then(({data}) => {
                commit('setNickname', payload.nickname)
                resolve(data)
            })
            .catch(error => {
                if (error.response.data.message) {
                    reject(error.response.data.message)
                } else if (error.response.data.code) {
                    reject(error.response.data.code)
                } else if (error.response.data.mensaje){
                    reject(error.response.data.mensaje.code)
                } else {
                    reject('Error Desconocido')
                }
            })
        });
    },
    rekognition({rootState, commit}, payload) {
        return new Promise((resolve, reject) => {
            Axios.post(`${rootState.api}api/authentication/recognition`, payload)
            .then(({data}) => {
                commit('setNickname', payload.nickname)
                resolve(data)
            })
            .catch(error => {
                console.log(error);
                if (error.response.data.message) {
                    reject(error.response.data.message)
                } else if (error.response.data.code) {
                    reject(error.response.data.code)
                } else if (error.response.data.mensaje){
                    reject(error.response.data.mensaje.code)
                } else {
                    reject('Error Desconocido')
                }
            })
        });
    }
}

export default {
    namespaced: true,
    state, 
    actions,
    mutations
}