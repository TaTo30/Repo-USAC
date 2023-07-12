<template>
  <div :style="{'width': picw + 'px', 'height': pich+ 'px'}" style="max-width: 1000px" class="relative-position ">
    <canvas v-show="false" class="absolute" :width="picw" :height="pich" ref="canvas" />
    <img class="absolute" :width="picw" :height="pich" :src="image.src" />
    <video class="absolute" v-if="showVideo" :width="picw" :height="pich"  @canplay="setCanvas"  ref="camera" autoplay />
    <div class="absolute-bottom">
         <div class="row items-center justify-center">
            <q-btn v-if="!image.src" class="q-mb-sm q-mx-xs" color="positive" icon="camera" unelevated round @click="takePicture" />
            <q-file @update:model-value="fileUpload" v-if="!image.src" label="Subir imagen" class="q-mb-sm q-mx-xs" dense rounded outlined accept=".png, .jpg" v-model="file" bg-color="white" />
            <q-btn v-if="image.src" class="q-mb-sm q-mx-xs" color="positive" icon="done" unelevated round @click="$emit('picture', image)" v-close-popup />
            <q-btn v-if="image.src" class="q-mb-sm q-mx-xs" color="primary" icon="replay" unelevated round @click="initCamera" />
         </div>
    </div>
  </div>
</template>

<script>
import {ref, onMounted} from 'vue'

export default {
    emits: ['picture'],
    setup(){
        const camera = ref(null)
        const canvas = ref(null)

        const pich = ref(0)
        const picw = ref(600)
        
        const showVideo = ref(true)

        const image = ref({
            src: '',
            ext: ''
        })
        const file = ref(null)


        const initCamera = () => {
            file.value = null
            showVideo.value = true
            image.value = {}
            navigator.mediaDevices.getUserMedia({video: true, audio: false})
                .then(stream => {
                    camera.value.srcObject = stream
                }).
                catch(error => {
                    alert("No se puede iniciar la camara en estos momentos")
                })
        }

        const setCanvas = (value) => {
            pich.value = camera.value.videoHeight / (camera.value.videoWidth/picw.value);
        }

        const fileUpload = (value) => {
            const reader = new FileReader()
            reader.onload = () => {
                const Oimage = new Image()
                Oimage.onload = () => {
                    pich.value = Oimage.naturalHeight / (Oimage.naturalWidth/picw.value)
                    showVideo.value = false
                    image.value = {
                        src: reader.result,
                        ext: value.name.split('.')[1]
                    }
                }
                Oimage.src = reader.result
            }
            reader.readAsDataURL(value)
        } 

        const takePicture = () => {
            showVideo.value = false
            var context = canvas.value.getContext('2d')
            context.drawImage(camera.value, 0,0, picw.value, pich.value)

            image.value = {
                src: canvas.value.toDataURL('image/png'),
                ext: 'png'
            }
        }

        onMounted(() => {
            initCamera()
        })

        return{
            file,
            camera,
            canvas,
            picw,
            pich,
            showVideo,
            image,
            initCamera,
            setCanvas,
            takePicture,
            fileUpload
        }
    }
}
</script>

<style>

</style>