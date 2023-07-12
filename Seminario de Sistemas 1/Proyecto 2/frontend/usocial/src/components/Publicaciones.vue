<template>
  <div class="row justify-center q-px-xl q-py-md ">
      <div class="column justify-center fit q-mx-xl q-px-xl">
        <div class="q-px-xl">
          <q-expansion-item
            dark
            header-class="bg-grey-10"
            class="bg-grey-10 rounded-borders"
            expand-separator
            label="Publicar algo"
            expand-icon="add"
            ref="EXP"
          >
          <div style="width: 80%" class="row items-center q-pa-md">
            <img width="500" :src="newpimage.src" />
          </div>
            <div class="row items-center q-pa-md">
              <div class="col-grow q-mx-sm">
                <q-input class="bg-white" v-model="newptext" type="textarea" label="¿En que estas pensando?" autogrow outlined />
              </div>
              <div class="q-ml-sm">
                <q-btn @click="takepic = true" color="positive" icon="add_a_photo" round unelevated />
              </div>
            </div>
            <div class="row justify-end">
              <q-btn :disable="!newpimage.src" class="q-ma-sm" label="Publicar" color="info" flat @click="publicar" />
            </div>
          </q-expansion-item>
        </div>
        <div class="q-px-xl">
          <div class="row justify-end q-my-xs"> 
          <q-select dark input-class="text-primary" clearable style="width: 30%" dense borderless v-model="filtro" :options="etiquetas" label="Etiquetas" dropdown-icon="filter_list"  />
          <q-btn class="q-mr-sm" color="white" icon="refresh" unelevated round flat @click="obtenerPublicaciones" />
          </div>
          <div v-for="(pub, index) in publicaciones" :key="index" >
            <q-card dark pub.labels class="q-my-md" v-if="filtro? pub.labels.map(value => {return value.Name}).indexOf(filtro) !== -1 : true">
            <img :src="pub.image">
            <q-card-section>
              <div class="text-h6">{{pub.owner}}</div>
              <div class="text-subtitle2 text-grey-8">{{pub.createdAt}}</div>
              <div>
                <q-chip v-for="(label, li) in pub.labels" :key="li" color="blue-grey-7" text-color="white" :label="label.Name" />
              </div>
            </q-card-section>
            <q-card-section>
              <div v-if="pub.text">
                {{pub.text}}
                <div v-if="detect.detect(pub.text, 1)[0][0] !== 'spanish'">
                  <div>
                    <q-btn color="info" label="Traducir Texto" flat @click="traducirTexto(pub)" />
                  </div>
                  <div class="q-ml-lg text-grey-8">
                    <q-btn v-if="pub.transtext" class="cursor-pointer" @click="pub.transtext = ''" icon="clear" round dense flat unelevated color="red" /> <span>{{pub.transtext}}</span><i class="text-grey-6" v-if="pub.transtext">(Traducido desde {{detect.detect(pub.text, 1)[0][0]}})</i>
                  </div>
                </div>
              </div>
            </q-card-section>
          </q-card>
          </div>
        </div>
      </div>   
  </div>
  <q-dialog v-model="takepic" >
       <TomarFoto @picture="setPicture" />
   </q-dialog>

</template>

<script>
import { ref } from "vue";
import { useStore } from "vuex";
import { useQuasar } from 'quasar'

import LanguageDetect from "languagedetect";


import TomarFoto from "../components/TomarFoto.vue";

export default {
  components: {
        TomarFoto
    },
  setup() {
    const $store = useStore()
    const $q = useQuasar()
    const detect = new LanguageDetect()

    const newptext = ref('')
    const newpimage = ref({src: '', ext: ''})

    const etiquetas = ref([])
    const publicaciones = ref([])

    const filtro = ref('')

    const EXP = ref(null)



    const traducirTexto = (value) => {
      $store.dispatch('publicaciones/traducir', {
        text: value.text
      })
      .then(data => {
        value.transtext = `${data.message.TranslatedText}`
      })
    }

    const obtenerPublicaciones = () => {
      $store.commit('publicaciones/setLoading', true)
      $store.dispatch('publicaciones/obtenerPublicaciones')
      .then(data => {

          let arrays = data.map(value => {
            if (value.labels) {
              return value.labels.map(label => label.Name)
            }
            return []
          })
          let labels = []
          for (const arr of arrays){
            for (let item of arr){
              labels.push(item)
            }
          }
          etiquetas.value = labels          
        
        publicaciones.value = data
        publicaciones.value.transtext = ''
      }).finally(() => $store.commit('publicaciones/setLoading', false))
    }

    const publicar = () => {
      $store.commit('publicaciones/setLoading', true)
      $store.dispatch('publicaciones/publicar', {
        nickname: $store.state.usuario.nickname,
        text: newptext.value,
        image: {
          extension: 'png',
          data: newpimage.value.src.split(',')[1]
        }
      }).then(data => {
        $q.notify({
          position: 'bottom-right',
          type: 'positive',
          message: 'Publicado'
        })
        newpimage.value.src = ''
        newptext.value = ''
        EXP.value.hide()
        obtenerPublicaciones()
      }).catch(() => $store.commit('publicaciones/setLoading', false))
    }

    const setPicture = (value) => {
        newpimage.value = value
    }

    obtenerPublicaciones()

    return {
      publicaciones,
      traducirTexto,
      newpimage,
      newptext,
      takepic: ref(false),
      setPicture,
      publicar,
      obtenerPublicaciones,
      EXP,
      filtro,
      etiquetas,
      detect
    }
  }

}
</script>

<style>

</style>