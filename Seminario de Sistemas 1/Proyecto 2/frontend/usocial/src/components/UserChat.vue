<template>
    <q-card dark style="width: 500px">
        <q-card-section>
            <q-item>
                <q-item-section top avatar>
                    <q-avatar color="primary" text-color="white"  >
                        <img :src="friend.image" />
                    </q-avatar>
                </q-item-section>
                <q-item-section>
                    <q-item-label>{{friend.nickname}}</q-item-label>
                </q-item-section>
            </q-item>
        </q-card-section>
        <q-separator spaced />
        <q-card-section>
            <div v-for="(msg, index) in sequence" :key="index">
                <q-chat-message
                    :name="msg.user"
                    :text="[msg.msg]"
                    :sent="msg.user === store.state.usuario.nickname? true : false"
                />
              
            </div>
            <div class="row items-center justify-center">
                <q-input dark @keypress.enter="sendMessage" class="q-mx-sm col-grow" outlined dense v-model="newmsg" type="text" label="Aa" />
                <q-btn class="q-mx-sm" color="white" icon="send" round unelevated flat @click="sendMessage" />
            </div>
        </q-card-section>
    </q-card>
</template>

<script>
import { io } from "socket.io-client";

import { ref, onUnmounted } from 'vue'
import { useStore } from "vuex";

export default {
    props: {
        friend: Object
    },
    setup(){
        const store = useStore()
        const socket = io(store.state.api)

        const newmsg = ref('')

        const sequence = ref([])

        const sendMessage = () => {
            socket.emit('chat', {user: store.state.usuario.nickname, msg: newmsg.value})
            newmsg.value = ''
        }

        socket.on('chat', (data) => {
            sequence.value.push(data)
        })

        onUnmounted(() => {
            socket.disconnect()
        })

        return {
            newmsg,
            sequence,
            store,
            sendMessage
        }
    }

}
</script>

<style>

</style>