import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      redirect: '/notes'
    },
    {
      path: '/notes',
      name: 'notes',
      component: HomeView,
    },
    {
      path: '/user',
      name: 'user',
      // route level code-splitting
      // this generates a separate chunk (About.[hash].js) for this route
      // which is lazy-loaded when the route is visited.
      component: () => import('../views/UserView.vue'),
    },
    {
      path: '/notes/new',
      name: 'new-note',
      component: () => import('@/components/notes/NewNote.vue')
    },
    {
      path: '/note/:id',
      name: 'note-detail',
      component: () => import('@/components/notes/DetailNote.vue'),
      props: true
    },
    {
      path: '/note/edit/:id',
      name: 'edit-note',
      component: () => import('@/components/notes/EditNote.vue'),
      props: true
    }
  ],
})

export default router
