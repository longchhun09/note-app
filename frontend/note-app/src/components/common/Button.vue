<template>
    <button :class="buttonClasses" :disabled="disabled" @click="$emit('click')">
        <component :is="getIconComponent()" v-if="icon" class="mr-1 h-4 w-4" />
        {{ text }}
    </button>
</template>

<script setup lang="ts">
import { computed, defineProps, defineEmits } from 'vue';
import * as LucideIcons from 'lucide-vue-next';

type ButtonVariant = 'primary' | 'secondary' | 'danger' | 'success';
type ButtonSize = 'sm' | 'md' | 'lg';

const props = defineProps({
    text: {
        type: String,
        default: 'Create New'
    },
    variant: {
        type: String as () => ButtonVariant,
        default: 'primary',
        validator: (value: string) => ['primary', 'secondary', 'danger', 'success'].includes(value)
    },
    size: {
        type: String as () => ButtonSize,
        default: 'md',
        validator: (value: string) => ['sm', 'md', 'lg'].includes(value)
    },
    icon: {
        type: String,
        default: 'Plus'
    },
    disabled: {
        type: Boolean,
        default: false
    }
});

defineEmits(['click']);

const buttonClasses = computed(() => {
    const baseClasses = 'inline-flex items-center justify-center rounded-md font-medium shadow-sm focus:outline-none focus:ring-2 focus:ring-offset-2 px-4 py-2 text-sm';

    // Variant-specific classes
    let variantClasses = '';

    switch (props.variant) {
        case 'primary':
            variantClasses = 'bg-blue-600 text-white hover:bg-blue-700 focus:ring-blue-500 disabled:bg-blue-300';
            break;
        case 'secondary':
            variantClasses = 'bg-gray-100 text-gray-800 hover:bg-gray-200 focus:ring-gray-400 disabled:bg-gray-100 disabled:text-gray-400';
            break;
        case 'danger':
            variantClasses = 'bg-red-600 text-white hover:bg-red-700 focus:ring-red-500 disabled:bg-red-300';
            break;
        case 'success':
            variantClasses = 'bg-green-600 text-white hover:bg-green-700 focus:ring-green-500 disabled:bg-green-300';
            break;
        default:
            variantClasses = 'bg-blue-600 text-white hover:bg-blue-700 focus:ring-blue-500 disabled:bg-blue-300';
    }

    // Disabled state
    const disabledClasses = props.disabled ? 'cursor-not-allowed opacity-75' : 'cursor-pointer';

    return `${baseClasses} ${variantClasses} ${disabledClasses}`;
});

const getIconComponent = () => {
    // Access the icon component from the imported Lucide icons
    if (props.icon && props.icon in LucideIcons) {
        return LucideIcons[props.icon as keyof typeof LucideIcons];
    }
    return null;
};
</script>
