import { store } from 'quasar/wrappers'
import { createStore } from 'vuex'

import usuario from './modules/usuario'
import amigo from './modules/amigo'
import publicaciones from "./modules/publicaciones";

/*
 * If not building with SSR mode, you can
 * directly export the Store instantiation;
 *
 * The function below can be async too; either use
 * async/await or return a Promise which resolves
 * with the Store instance.
 */

export default store(function (/* { ssrContext } */) {
  const Store = createStore({
    state: {
      api: process.env.API_URL,
      api_translate: process.env.AWS_API,
      api_covid: process.env.COVID_API
    },
    modules: {
      usuario,
      amigo,
      publicaciones
    },

    // enable strict mode (adds overhead!)
    // for dev mode and --debug builds only
    strict: process.env.DEBUGGING
  })

  return Store
})
