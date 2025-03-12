import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'
import NotesLayout from '../views/NotesLayout.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      redirect: '/notes'
    },
    {
      path: '/notes',
      component: NotesLayout,
      children: [
        {
          path: '',
          name: 'notes',
          component: () => import('../views/NoteWelcome.vue')
        },
        {
          path: ':id',
          name: 'note-detail',
          component: () => import('@/components/notes/NotePanel.vue'),
          props: route => ({ 
            id: Number(route.params.id),
            editMode: false
          })
        },
        {
          path: ':id/edit',
          name: 'edit-note',
          component: () => import('@/components/notes/NotePanel.vue'),
          props: route => ({ 
            id: Number(route.params.id),
            editMode: true
          })
        },
        {
          path: 'new',
          name: 'new-note',
          component: () => import('@/components/notes/NotePanel.vue'),
          props: { isNew: true }
        },
      ]
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
      path: '/login',
      name: 'login',
      component: () => import('../views/LoginView.vue')
    },
  ],
})

export default router
